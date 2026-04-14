/**
 * getUserMedia só existe em contexto seguro (HTTPS ou localhost).
 * Em http://192.168.x.x o navigator.mediaDevices fica undefined — usar InputFile+capture no Blazor.
 */
export function canUseLivePreview() {
    return !!(navigator.mediaDevices && typeof navigator.mediaDevices.getUserMedia === 'function');
}

export async function startPreview(video) {
    if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
        throw new Error(
            'PREVIEW_HTTP: A pré-visualização ao vivo não está disponível neste endereço (precisa HTTPS ou localhost). Use o botão «Tirar / escolher foto».'
        );
    }
    const stream = await navigator.mediaDevices.getUserMedia({
        video: { facingMode: { ideal: 'environment' }, width: { ideal: 1280 }, height: { ideal: 720 } },
        audio: false
    });
    video.srcObject = stream;
    await video.play();
    return true;
}

export function stopPreview(video) {
    const s = video.srcObject;
    if (s) {
        s.getTracks().forEach(t => t.stop());
        video.srcObject = null;
    }
}

export function captureToDataUrl(video, quality, maxDimension) {
    const w = video.videoWidth;
    const h = video.videoHeight;
    if (!w || !h) return null;

    const maxSize = Number.isFinite(maxDimension) && maxDimension > 0 ? maxDimension : 1280;
    const scale = Math.min(1, maxSize / Math.max(w, h));
    const targetW = Math.max(1, Math.round(w * scale));
    const targetH = Math.max(1, Math.round(h * scale));

    const canvas = document.createElement('canvas');
    canvas.width = targetW;
    canvas.height = targetH;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(video, 0, 0, targetW, targetH);
    return canvas.toDataURL('image/jpeg', quality ?? 0.75);
}

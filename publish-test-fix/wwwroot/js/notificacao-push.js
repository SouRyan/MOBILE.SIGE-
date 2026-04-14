/* Notificações push (browser Notification API) para o PWA SIGE */
window.SigeNotificacao = {

    /** Pede permissão ao usuário. Retorna 'granted', 'denied' ou 'default'. */
    requestPermission: async function () {
        if (!('Notification' in window)) return 'unsupported';
        if (Notification.permission === 'granted') return 'granted';
        return await Notification.requestPermission();
    },

    /** Retorna a permissão atual sem pedir. */
    getPermission: function () {
        if (!('Notification' in window)) return 'unsupported';
        return Notification.permission;
    },

    /**
     * Exibe uma notificação nativa via Service Worker (persiste mesmo com app em background).
     * @param {string} titulo
     * @param {string} mensagem
     * @param {string} icone  - caminho do ícone (ex: '/icon-192.png')
     * @param {string} url    - URL para abrir ao clicar
     * @param {string} tag    - tag única para evitar duplicatas
     */
    show: async function (titulo, mensagem, icone, url, tag) {
        if (!('Notification' in window)) return;
        if (Notification.permission !== 'granted') return;

        // Preferir Service Worker (funciona em background no mobile)
        if ('serviceWorker' in navigator) {
            try {
                var reg = await navigator.serviceWorker.ready;
                await reg.showNotification(titulo, {
                    body: mensagem,
                    icon: icone || '/icon-192.png',
                    badge: '/icon-192.png',
                    tag: tag || ('sige-' + Date.now()),
                    data: { url: url || '/dashboard' },
                    vibrate: [200, 100, 200],
                    requireInteraction: false
                });
                return;
            } catch (e) { /* fallback abaixo */ }
        }

        // Fallback: Notification API direta (não funciona em background)
        new Notification(titulo, {
            body: mensagem,
            icon: icone || '/icon-192.png',
            tag: tag || ('sige-' + Date.now())
        });
    },

    /** Salva o último ID de notificação visto no localStorage. */
    getLastSeenId: function () {
        return parseInt(localStorage.getItem('sige-notif-last-id') || '0', 10);
    },

    setLastSeenId: function (id) {
        localStorage.setItem('sige-notif-last-id', id.toString());
    }
};

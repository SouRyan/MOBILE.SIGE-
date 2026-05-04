# MOBILE-SIGE

Aplicação mobile/PWA do SIGE para uso operacional em campo e produção, desenvolvida com Blazor Server.

O projeto consiste em um sistema completo para gerenciar e centralizar informações de empresas do ramo de esquadrias de alumínio, conectando operação de campo, produção e gestão administrativa em um único ecossistema.
Na prática, ele organiza dados de obras, famílias e caixilhos, facilita o acompanhamento do andamento da produção, permite registro de medição com foto e oferece visão consolidada para tomada de decisão.
Este módulo mobile é focado na execução operacional diária, com interface rápida para equipas em obra e fábrica.

## Visão geral

O `MOBILE-SIGE` é a aplicação usada por equipas de obra/produção para:

- autenticação de utilizadores;
- acompanhamento de dashboard operacional;
- consulta de obras e famílias;
- medição com foto;
- apontamentos de produção;
- gestão de conta/perfil.

## Contexto do ecossistema

Este projeto faz parte de um ecossistema com 3 aplicações:

- `MOBILE-SIGE` (este repositório): interface mobile/PWA focada na operação diária (campo, medição e produção).
- `API-SIGE`: backend central (autenticação, regras de negócio, dados).
- `GenProd`: módulo web MVC voltado à administração e gestão geral.

A arquitetura base é:

1. O utilizador entra no `MOBILE-SIGE`.
2. O app autentica e consulta dados na `API-SIGE`.
3. Em fluxos específicos, o app também integra recursos do `GenProd`.

## Integração com os demais módulos

- Consome a API principal em `API-SIGE` para autenticação e dados de negócio.
- Integra o módulo web `GenProd` para fluxo de medição/fotos.

Leitura recomendada (aprofundamento):

- [API-SIGE](https://github.com/SouRyan/API-SIGE)
- [GenProd](https://github.com/ViniciusWRocha/GenProd)

## Stack

- .NET 10
- ASP.NET Core Blazor Web App (Interactive Server)
- PWA (manifest + service worker)

## Execução local

Pré-requisito: .NET SDK 10 instalado.

```bash
dotnet restore
dotnet run
```

URL local padrão: `http://localhost:5075`

## Configuração

Arquivo principal: `appsettings.json`

- `ApiSige:BaseUrl`: URL da API do `API-SIGE`
- `GerenciamentoWeb:BaseUrl`: URL do módulo `GenProd` (serviço de medição)

## Fluxo de integração (resumo)

1. Utilizador faz login no `MOBILE-SIGE`.
2. `MOBILE-SIGE` autentica via `API-SIGE` (JWT).
3. Telas de negócio consultam endpoints do `API-SIGE`.
4. Fluxos específicos de medição/foto podem chamar o `GenProd`.

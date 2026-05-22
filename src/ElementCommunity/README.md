# ElementCommunity

ElementCommunity is the new community mainline under `community/src/ElementCommunity`.

It does not reuse the legacy community project files, UI, package names, or startup path. The first C1 skeleton contains:

- `ElementCommunity.App`: Blazor Web App host
- `ElementCommunity.Domain`: community domain records
- `ElementCommunity.Infrastructure`: static read model for the first runnable skeleton
- `ElementCommunity.Components`: reusable community UI built with Element-Blazor `El*` controls

Run locally:

```powershell
.\start-community.ps1
```

Default URL:

```text
http://localhost:5096
```

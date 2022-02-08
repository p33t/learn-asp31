Project for learning about ASP.NET Core
=======================================

# Notes
## Configuration Override
The default setting in `appSettings.Development.json` can be overridden in two ways:
- For running in IDE, one can edit `lauchSettings.json` with an entry under `environmentVariables` with key `CustomSection1__CustomSetting11`
- For running from command line, one can supply via an environment variable named `CustomSection1__CustomSetting11`  
  e.g. Add this to `~/.bashrc`:
```bash
export CustomSection1__CustomSetting11="Value from .bashrc"
```
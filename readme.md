<!-- <p align="center"><img src="./include/varlet.png" height="80px"></p> -->

# Varlet

Varlet is a a web development environment for minimalists, inspired from [Laravel Valet](https://laravel.com/docs/valet)
and [Laragon](https://laragon.org). Varlet only includes PHP, Composer, and HTTP web server. If you want to use databases
like PostgreSQL, MariaDB/MySQL, Redis, you need to install them separately.

Varlet is made for you, the developers who like to work in the terminal, like me!

## What's in the box?

- PHP 7.2 + 7.3
- Apache HTTPD
- Composer
- xDebug
- PHP Redis
- ImageMagick
- ionCube loader
- Phalcon PHP extension
- Mailhog + mhsendmail
- Adminer db manager
- Automatic https

## Quick Start

To install Varlet you need [dotNet Framework](https://dotnet.microsoft.com/download/dotnet-framework) >= 4.5.2,
then download [latest release](https://github.com/riipandi/varlet/releases) and run installation file.

Varlet doesn't have `park` command like Laravel Valet does. Your project files can stored at:
`installation_path\htdocs`.

Or, you can use the `varlet link` command and place your project files in any directory you want.

## Building Packages

If you want to build by yourself you will need:

```
Build Tools for Visual Studio 2019  : https://visualstudio.microsoft.com/downloads/#vstool-2019-family
Inno Setup (creating the installer) : http://www.jrsoftware.org/isdl.php
```

Then execute `setup.bat` and enjoy a cup of coffee.

<!-- ## Varlet Commands

| Command                      | Description
| :--------------------------- | :----------
| `varlet link`                  | Create virtualhost and serving the site
| `varlet unlink`                | Remove virtualhost
| `varlet forget`                | Remove both of virtualhost http and https
| `varlet start`                 | Start Httpd service
| `varlet stop`                  | Stop Httpd service
| `varlet restart`               | Restart Httpd service
| `varlet status`                | View site link status
| `varlet service-status`        | View services status
| `varlet switch-php _version_`  | Switch PHP version `7.4/7.3/7.2` -->

## License

Varlet is free software: you can distribute it and or modify it according to the license provided.
Varlet is a compilation of free software, it's free of charge and it's free to copy under the terms
of the [Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/). Please check every
single licence of the contained products to get an overview of what is, and what isn't, allowed.
In the case of commercial use please take a look at the product licences (_especially MySQL_),
from the my point of view commercial use is also free.

Read the [licence file](./license.txt) file for the full Varlet license text.

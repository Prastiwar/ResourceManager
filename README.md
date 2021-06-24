# RPGDataEditor

***RPGDataEditor is software tool to visualise and edit json files that contains rpg mechanic data like quests and dialogues***

[![GitHub last commit](https://img.shields.io/github/last-commit/Prastiwar/RPGDataEditor.svg?label=Updated&style=flat-square&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/commits/master)
[![license](https://img.shields.io/github/license/Prastiwar/RPGDataEditor.svg?style=flat-square&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/blob/master/LICENSE)
[![GitHub forks](https://img.shields.io/github/forks/Prastiwar/RPGDataEditor.svg?style=social&label=Fork&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/fork)
[![GitHub stars](https://img.shields.io/github/stars/Prastiwar/RPGDataEditor.svg?style=social&label=★Star&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/stargazers)
[![GitHub watchers](https://img.shields.io/github/watchers/Prastiwar/RPGDataEditor.svg?style=social&labelWatcher&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/watchers)
[![GitHub contributors](https://img.shields.io/github/contributors/Prastiwar/RPGDataEditor.svg?style=social&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/contributors)

![GitHub repo size in bytes](https://img.shields.io/github/repo-size/Prastiwar/RPGDataEditor.svg?style=flat-square&longCache=true)
[![GitHub issues](https://img.shields.io/github/issues/Prastiwar/RPGDataEditor.svg?style=flat-square&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/issues)
[![GitHub closed issues](https://img.shields.io/github/issues-closed/Prastiwar/RPGDataEditor.svg?style=flat-square&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/Prastiwar/RPGDataEditor.svg?style=flat-square&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/pulls)
[![GitHub closed pull requests](https://img.shields.io/github/issues-pr-closed/Prastiwar/RPGDataEditor.svg?style=flat-square&longCache=true)](https://github.com/Prastiwar/RPGDataEditor/pulls)

## Connection

Connection to data source allows to manipulate data within different storage like ftp, sql database or local OS file system   

### Supported Data Sources:

- #### Local

This data source connects to local os file system with provided FolderPath - full path to root of all resources. Before using this connection make sure your account have read/write permissions in given folder path.

- #### FTP

This data source connects to file system using FTP. It needs host, username and password to connect desired server. If port is set to 0, client will try to find correct port. If username and password will not be provided, it will connect to server as anonymous user, so make sure your server supports it. You can provide relative path to server root to use desired location. Before connecting to server make sure your account has read/write permissions to access given path.

- #### SQL

This data source connects to SQL database using connection string. Before using this connection make sure your login has select/insert/update/delete permissions to needed tables.

## Default Data Architecture

RPGDataEditor uses 3 resources: Npc, Quest, Dialogue

TBA

## Extending RPGDataEditor.Wpf

There are few important things you need to setup before succesfully launch RPGDataEditor.Wpf extension app
1. Serialization - you must configure serialization module so each resource that extends base resource, must have own converter, so additional properties will be also serialized
2. Validation - you must configure validation module so each resource must have own IValidator<> 
3. Ignore base types - TBA

## Contributing

You can freely contribute with us by reporting issues and making pull requests!
Please read [CONTRIBUTING.md](https://github.com/Prastiwar/RPGDataEditor/blob/master/.github/CONTRIBUTING.md) for details on contributing.

## Authors

* ![Avatar](https://avatars3.githubusercontent.com/u/33370172?s=40&v=4)  [**Tomasz Piowczyk**](https://github.com/Prastiwar) - *The Creator*
See also the list of [contributors](https://github.com/Prastiwar/RPGDataEditor/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/Prastiwar/RPGDataEditor/blob/master/LICENSE) file for details.

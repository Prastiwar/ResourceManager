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

## Extending RPGDataEditor.Wpf

You can see in samples how to properly exend it, there are few important things you need to setup before succesfully creating RPGDataEditor app
1. Models - Quest, dialogues, tasks, npcs, any resource you want to manage you need to define on your own. **Main resources need to implement `IIdentifiable` interface**
2. Serialization - you must configure serialization module so each resource that extends base resource, must have own converter, so additional properties will be also serialized
3. Validation - you must configure validation module so each resource must have own IValidator<> 
4. DefaultDbContext - you should configure model creation with own DbContext - it's required only when you want to support SQL connection
   
5. WPF project extension
   
- [x] Make sure your `App.cs` extend `RpgDataEditorApplication` 
- [x] Create `RegionTabModuleBase` implementation 
- [x] Register your `RegionTabModuleBase` in `App.cs` like `protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();` 
- [x] Build your data sources in `protected override void ConfigureDataSources(IConfigurableDataSourceBuilder builder)` 
- [x] Configure json settings in `protected override JsonSerializerSettings CreateJsonSettings()` (add your converters) 

- [x] Add static converters to `App.xaml` with `<ResourceDictionary Source="pack://application:,,,/RPGDataEditor.Wpf;component/Themes/Converters.xaml" />` 
- [x] Add default themes to `App.xaml` with `<ResourceDictionary Source="pack://application:,,,/RPGDataEditor.Wpf;component/Themes/Generic.xaml" />` 

ViewModels
- [x] Any editable resource can just extend `ModelDialogViewModel<>`
- [x] Any resource that has it's own Tab can just extend `ModelsManagerViewModel<>` or `CategoryModelsManagerViewModel<>`

Optional AutoControl system in Views
- [x] Types that can be auto templated should extend AutoTemplate 
- [x] extend `DefaultAutoTemplateProvider` and register your AutoTemplate's

## Sample

RPGDataEditor.Sample uses 3 resources: Npc, Quest, Dialogue

More description SOON

## Contributing

You can freely contribute with us by reporting issues and making pull requests!
Please read [CONTRIBUTING.md](https://github.com/Prastiwar/RPGDataEditor/blob/master/.github/CONTRIBUTING.md) for details on contributing.

## Authors

* ![Avatar](https://avatars3.githubusercontent.com/u/33370172?s=40&v=4)  [**Tomasz Piowczyk**](https://github.com/Prastiwar) - *The Creator*
See also the list of [contributors](https://github.com/Prastiwar/RPGDataEditor/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/Prastiwar/RPGDataEditor/blob/master/LICENSE) file for details.

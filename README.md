# BuilderScenario

 BuilderScenario - гибкий и легко расширяемый маханизм для соззания и выполнения сценариев сборки проектов Unity3D.

 ![Окно редактора конфигурации](https://raw.githubusercontent.com/CTAPbIuKODEP/BuilderScenario/master/docs/exampleview.png)

## Ключевые особенности:

 - Основан на системе неделимых действий(шаги сборки) которые можно компоновать в цели сборки
 - Легкое добавление шагов сборки
 - Порядка 15 встреных необходимых шагов сборки
 - Поддержка TeamCity
 - Логи сборки
 - Удобный UI для редактирование конфигурации сборки
 - yaml файл конфигурайии сборки
 - bash/cmd frendly

## Пример конфигурационного файла

 Пример получен из сохранения конфигурации в скриншоте выше.

```yaml
Name: Example
AbsolutePathForBuilds: '{PROJECT_PATH}Build/'
Version: 0.1.0
Build: 43
BuildResultPath: '{PATH}'

ActionsBeforeTargets:
- !PushScriptDefinition
  CanAction: true

ActionsAfterTargets:
- !PopScriptDefinition
  CanAction: true

Targets:
- IsBuilding: true
  TargetName: windows client
  Actions:
  - !BuildAction
    Path: '{PATH}Windows/{NAME}.exe'
    Scenes:
    - Assets/client.unity
    Target: StandaloneWindows64
    Options: ShowBuiltPlayer
    DefinedSymbols: CLIENT
    CanAction: false
- IsBuilding: true
  TargetName: linux client
  Actions:
  - !BuildAction
    Path: '{PATH}Windows/{NAME}.exe'
    Scenes:
    - Assets/start.unity
    Target: StandaloneLinux
    Options: ShowBuiltPlayer
    DefinedSymbols: CLIENT
    CanAction: false
```

 В примере перечислены имя проекта, пути для сборки, версию, сквозной номер билда(если запускать через панель меню в Unity, то будет инкрементится автоматически), выполняемые шаги до и после всех целей, и соответственно цели и их шаги.
 
 В случае примера имеем 2 цели для сборки клиентов н 2 полатформы.

## Запуск сборки через cmd/bash

** cooming soon... **
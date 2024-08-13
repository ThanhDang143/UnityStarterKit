# BakingSheet Implemented
- Implement BakingSheet by Cathei for my needs.
- Origin: https://github.com/cathei/BakingSheet.
- Using to convert `Excel` to `ScripableObject`.
- Require `OdinInspector`

## Changelog v0.0.2
- Add `DataConstant`
## Changelog v0.0.1
- Support convert `Excel` to `Scriptable Object`

## How to use.
### Create and Use DataModel
- Call `InitialData` to prepare data before use anything.
- Whenever a new DataModel is introduced, a corresponding property declaration must be added inside region `Declare`. `Ex: public DemoSheet Demo { get; private set; }`
- After that, use method ForEach to cache DataSheet inside region `CacheData` inside function `CacheData()`. `Ex: Demo.ForEach(d => result.Add(d.Id, d));`
- Finally, use functions inside `DataManager` to interact with the data.

### Update data
- After edit raw data (Excel, GGSheet...).
- Open Data window by `Ctrl + G` or `Tools/Data Manager`.
- Choose input and output path if needs.
- Press UpdateData button.
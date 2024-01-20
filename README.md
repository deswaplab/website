# Deswap blazor APP


## build

```shell
npx tailwindcss -i .\Styles\app.css -o .\wwwroot\css\app.css --watch
dotnet watch
```

## Tasks

- [ ] 本地化[see doc](https://learn.microsoft.com/en-us/aspnet/core/blazor/globalization-localization?view=aspnetcore-8.0)
- [ ] mint页，token pair 默认select的问题
- [ ] 实现期权拆分功能
- [ ] 实现期货交易功能
- [ ] reservoir 接口测试环境总是报错，考虑切换到opensea的接口(待定，免费的api少)
- [ ] 把reservoir方法封装，便于未来替换（进行中）
- [ ] 优化日志
- [ ] onblur问题已经影响到了手机上菜单栏的点击
- [ ] 手机上菜单栏 z-index absolute 问题未解决
- [ ] mint时给出当前推荐价格
- [ ] nft图片展示的时间时区有点问题

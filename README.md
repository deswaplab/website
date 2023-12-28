# Deswap blazor APP


## build

```shell
npm tailwindcss -i .\Styles\app.css -o .\wwwroot\css\app.css --watch
dotnet watch
```

## Tasks

- [ ] 本地化[see doc](https://learn.microsoft.com/en-us/aspnet/core/blazor/globalization-localization?view=aspnetcore-8.0)
- [ ] 切换地址的时候，状态同步有点问题
- [ ] 加上网络选择菜单
- [ ] mint表单优化校验
- [ ] mint 表单 select token pair处使用的是 onchange事件，默认选中了 WETH/USDC 导致没触发这个事件，导致余额没更新
- [ ] flowbite的js可以考虑不引用，因为没用到

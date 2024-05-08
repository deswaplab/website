# Deswap blazor APP

## build

```shell
npm install -g tailwindcss --upgrade
npx tailwindcss -i .\Styles\app.css -o .\wwwroot\css\app.css --watch
dotnet watch
```

## Tasks

- [ ] appnav 上面加一个搜索框
- [ ] 重复代码优化，统一风格
- [ ] 选择合约时，内置一些已知的合约地址
- [ ] 测试
- [ ] 提交表单时强校验余额，减少出错机会

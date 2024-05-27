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
- [ ] 手机上添加未知网络时，会报错，切换不过去
- [ ] 详情页切换到popover后，弹出变窄了，需要排查下
- [ ] Roulette 弹窗改为popover
- [ ] popover 点击后自动关闭(hide)
- [ ] 列表页url优化，和详情页统一
- [ ] 彩票刷新元数据
- [ ] 是否可以把彩票的buy改为自动去opensea挂单？
- [ ] 加上 PWA 支持

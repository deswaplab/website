# Deswap blazor APP

## build

```shell
npm install -g tailwindcss --upgrade
npx tailwindcss -i .\Styles\app.css -o .\wwwroot\css\app.css --watch
dotnet watch

# for ipfs build, disable compression to reduce size
dotnet publish -p:CompressionEnabled=false
```

## Tasks

- [ ] appnav 上面加一个搜索框
- [ ] 重复代码优化，统一风格
- [ ] 选择合约时，内置一些已知的合约地址
- [ ] 测试
- [ ] 提交表单时强校验余额，减少出错机会
- [ ] 手机上添加未知网络时，会报错，切换不过去
- [ ] 详情页切换到popover后，弹出变窄了，需要排查下
- [ ] 是否可以把彩票的buy改为自动去opensea挂单？
- [ ] 思考如何改进writing，使得更符合开放互联网概念
- [ ] 普通人可以创建自己的公众号，将作品投递到这个公众号里

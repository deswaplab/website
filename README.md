# Deswap blazor APP


## build

```shell
npx tailwindcss -i .\Styles\app.css -o .\wwwroot\css\app.css --watch
dotnet watch
```

## Tasks

- [ ] reservoir 接口测试环境总是报错，考虑切换到opensea的接口(待定，免费的api少)
- [ ] 把reservoir方法封装，便于未来替换（进行中）
- [ ] onblur问题已经影响到了手机上菜单栏的点击
- [ ] options回归erc721，删除USDC合约
- [ ] gitbook 文档
- [ ] 不要在每个页面都监听metamask的事件，统一操作

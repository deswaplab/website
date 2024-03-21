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
- [ ] 找到办法如何交易moonbase alpha上的NFT
- [ ] 做一个合约详情页，避免依赖nft接口(难做好，在单独分支)
- [ ] 切换网络时，遇到未知网络自动添加rpc信息到metamask钱包
- [ ] https://devnet.neonft.market/

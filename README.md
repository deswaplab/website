# Deswap blazor APP


## build

```shell
npx tailwindcss -i .\Styles\app.css -o .\wwwroot\css\app.css --watch
dotnet watch
```

## Tasks

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
- [ ] 做一个资产筛选页，集成扫货凑单功能
- [ ] 接入ERC1155合约，优化mint界面，减少手动输入的步骤
- [ ] 重新规划产品组织，加上新的合约介绍
- [ ] options回归erc721
- [ ] 手动触发metadata更新

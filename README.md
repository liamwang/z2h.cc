<p align='center'>
  <img src='https://user-images.githubusercontent.com/5000396/168056715-bae0fc60-f1e5-4ccc-891c-0d9ac91f3520.png' alt='Vitesse - Opinionated Vite Starter Template' width='60' style="border-radius: 8px;"/>
</p>

<h3 align='center'>
<a href="https://z2h.cc/">z2h.cc</a>
</h6>

<p align='center'>
基于某盘实现的轻小图床
<p/>

<br>

## 特点

- ⚡️前端使用 [Vue 3](https://github.com/vuejs/vue-next), [TypeScript](https://www.typescriptlang.org/), [Vite](https://github.com/vitejs/vite) 等, 后端使用 [.NET 6](https://github.com/microsoft/dotnet)

- 🗂 前端基于文件路由，零配置

- 📦 前端组件按需自动引入

- 🔥 后端使用 Minimal API，干净无第三方组件

- ☁️ 免费 fly.io 部署

<br>

## 本地开发

### Clone 源码到本地

```bash
git clone https://github.com/liamwang/z2h.cc.git
cd z2h.cc
pnpm i # If you don't have pnpm installed, run: npm install -g pnpm
```

### 运行起来

```bash
# 只运行前端（将调用 z2h.cc 的开放 API）
pnpm dev:client

# 或者，在 appsettings.json 中配置你自己的城通账号
# 同时运行前端和后端
pnpm dev
```



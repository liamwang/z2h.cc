<p align='center'>
  <img src='https://user-images.githubusercontent.com/5000396/168504143-a3e3179c-c2ae-45e2-b8ab-bdb8c113ddd5.png' alt='Vitesse - Opinionated Vite Starter Template' width='400' style="border-radius: 8px;"/>
</p>

<h3 align='center'>
<a href="https://z2h.cc/">z2h.cc</a>
</h6>

<p align='center'>
基于某盘实现的轻小图床
<p/>

<br>

## 特点

- ⚡️ 前端使用 [Vue 3](https://github.com/vuejs/vue-next), [TypeScript](https://www.typescriptlang.org/), [Vite](https://github.com/vitejs/vite) 等

- 🤙🏻 后端使用 [.NET 6](https://github.com/microsoft/dotnet)

- 📦 前端基于文件路由，组件按需自动引入

- 🔥 后端使用 Minimal API，干净无第三方组件

- ☁️ 网站部署在 fly.io，国内访问服务器网速可能会很慢

- 😃 目前基于城通云盘的 API 做的文件存储和直接下载服务实现图床功能

- 🌍 后面将陆续增加对阿里云OSS、腾讯云COS等上传代理服务，便于 ShareX 配置

注意：本项目供大家学习使用，不推荐自行搭建整套系统（某云盘的直接下载服务是收费的，我是远古时期便宜的时候买的永久会员）

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
# 方式一（推荐）
# 运行下面命令运行前端，将调用 z2h.cc 的后端开放 API
pnpm dev:client

# 方式二（不推荐）
# 如果你有自己的某云盘账号，在 appsettings.json 中配置你的 Token 和上传目录
# 运行下面命令，同时运行前端和后端
pnpm dev
```



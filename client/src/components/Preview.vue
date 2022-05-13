<script setup lang="ts">
defineProps<{ file: FileResult; tag: string }>()
</script>

<template>
  <component :is="tag" class="preview">
    <img :src="file.url" class="rounded-t">
    <div class="relative h-10" :class="file.loading ? 'loading' : ''">
      <div v-if="file.loading">
        正在上传...
      </div>
      <div v-else>
        <span v-if="file.code === 0">
          <input id="input" class=" px-2 py-1 border rounded outline-none border-gray-200 dark:border-gray-700 text-sm">
          <button class="m-3 text-sm btn" @click="() => { }">
            复制
          </button>
        </span>
        <span v-else>
          Error
        </span>
      </div>
    </div>
  </component>
</template>

<style scoped lang="less">
.preview {
  display: flex;
  flex-direction: column;
  align-content: center;
  width: 600px;
  position: relative;
  background: #f3f7fa;
  border: 2px dashed #ddd;
  border-radius: 10px;
  margin: 20px auto;

  img {
    max-width: 100%;
    max-height: 300px;
    display: block;
  }

  .loading {
    animation: pulse 1.5s linear 0s infinite;
    color: #666;
    background: #fff;
  }
}

@keyframes pulse {
  0% {
    opacity: 1;
    // color: #999;
    // background: #fff;
  }

  50% {
    opacity: 0.7;
    // color: #555;
    // background: #ddd;
  }

  100% {
    opacity: 1;
    // color: #999;
    // background: #fff;
  }
}
</style>

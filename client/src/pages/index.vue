<script setup lang="ts">
import { toast, useUploader } from '~/composables'

const { upload, result } = useUploader()

function onFilesDroped(files: any[]) {
  upload(files[0])
}

function onFileChange(e) {
  upload(e.target.files[0])
  e.target.value = null
}

async function onCopyClick() {
  await navigator.clipboard.writeText(result.url!)
  toast.show('已复制')
}
</script>

<template>
  <div class=" max-w-prose px-5 mx-auto">
    <DropZone v-slot="{ dropZoneActive }" class="drop-area w-10/12 h-60 mx-auto flex-center flex-col rounded-lg"
      @files-dropped="onFilesDroped">
      <div class="i-ri-upload-cloud-line inline-block h-20 w-40 opacity-10 -mt-2" />
      <div v-if="dropZoneActive">
        <span>松开上传</span>
      </div>
      <div v-else>
        <div class="text-gray-400 dark:text-gray-600 mb-3 mt-3">
          将图片拖至此处上传
        </div>
        <label for="file-input" class="btn">
          点击上传
          <input id="file-input" type="file" @change="onFileChange">
        </label>
      </div>
    </DropZone>
    <div class="w-10/12 mx-auto my-5">
      <div v-if="result.code && result.message" class="text-sm text-red-500">
        {{ result.message }}
      </div>
      <Spin v-if="result.loading" class="text-gray-500 text-lg" />
      <div v-if="result.code === 0" class="px-2 flex justify-between">
        <input readonly :value="result.url"
          class="flex-1 px-2 py-1 text-sm border-2 rounded outline-none bg-transparent border-gray-300 dark:border-gray-700">
        <button class="btn ml-1" @click="onCopyClick">
          复制
        </button>
      </div>
    </div>
    <div v-if="result.url">
      <img :src="result.url" class="rounded-lg inline-block" :class="result.loading ? 'loading' : ''">
    </div>
    <p class="text-sm text-gray-400 dark:text-gray-600 flex-center mt-5">
      <!-- <span class="i-ri-error-warning-line inline-block mr-0.5" /> -->
      注意：当前为测试版本，数据只保留一周
    </p>
  </div>
</template>

<style scoped lang="less">
.drop-area {
  transition: .2s ease;
  background: #f3f7fa;
  border: 2px dashed #ddd;

  &[data-active=true] {
    background: #f0f4f8;
    border-color: #999;
  }

  input[type=file]:not(:focus-visible) {
    position: absolute !important;
    width: 1px !important;
    height: 1px !important;
    padding: 0 !important;
    margin: -1px !important;
    overflow: hidden !important;
    clip: rect(0, 0, 0, 0) !important;
    white-space: nowrap !important;
    border: 0 !important;
  }
}

img.loading {
  animation: pulse 1.5s linear 0s infinite;
  color: #666;
  background: #fff;
}

@keyframes pulse {
  0% {
    opacity: 1;
  }

  50% {
    opacity: 0.6;
  }

  100% {
    opacity: 1;
  }
}

.dark {
  .drop-area {
    background: #161a1e;
    border: 2px dashed #555;

    &[data-active=true] {
      background: #181c1f;
      border-color: #999;
    }
  }
}
</style>

<script setup lang="ts">
import { useUploader } from '~/composables'

const { upload, results } = useUploader()

function onFilesDroped(files: any[]) {
  upload(files[0])
}

function onInputChange(e) {
  upload(e.target.files[0])
  e.target.value = null
}
</script>

<template>
  <div>
    <DropZone v-slot="{ dropZoneActive }" class="drop-area" @files-dropped="onFilesDroped">
      <div class="i-ri-upload-cloud-line inline-block h-20 w-40 opacity-10 -mt-2" />
      <div v-if="dropZoneActive">
        <span>松开上传</span>
      </div>
      <div v-else>
        <div class="text-gray-400 dark:text-gray-600 mb-3 mt-4">
          将图片拖至此处上传
        </div>
        <label for="file-input" class="btn">
          点击上传
          <input id="file-input" type="file" @change="onInputChange">
        </label>
      </div>
    </DropZone>
    <ul v-show="results.length" class="image-list">
      <Preview v-for="file of results" :key="file.id" :file="file" tag="li" />
    </ul>
  </div>
</template>

<style scoped lang="less">
.drop-area {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 600px;
  height: 300px;

  border-radius: 10px;
  margin: 0 auto;

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

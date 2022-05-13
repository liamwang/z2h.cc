export function useUploader() {
  const result = reactive<UploadResult>({
    loading: false,
    url: '',
    code: -1,
    message: '',
  })

  async function upload(file: Blob) {
    Object.assign(result, {
      loading: true,
      url: URL.createObjectURL(file),
      code: -1,
      message: '',
    })

    const formData = new FormData()
    formData.append('file', file)
    formData.append('appId', import.meta.env.VITE_APP_ID)
    formData.append('appToken', import.meta.env.VITE_APP_TOKEN)

    try {
      const data = await fetch(import.meta.env.VITE_UPLOAD_URL, {
        method: 'POST',
        body: formData,
      }).then(res => res.json())
      Object.assign(result, {
        loading: false,
        url: data.url,
        code: data.code,
        message: data.message || '上传成功',
      })
    }
    catch (e) {
      console.error(e)
      Object.assign(result, { code: 1, message: '上传失败，请重试', loading: false })
    }
  }

  return { upload, result }
}


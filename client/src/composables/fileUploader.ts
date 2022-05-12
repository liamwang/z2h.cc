export async function uploadFile(file) {
  const loading = ref(true)

  const formData = new FormData()
  formData.append('file', file)
  formData.append('appId', import.meta.env.VITE_APP_ID)
  formData.append('appToken', import.meta.env.VITE_APP_TOKEN)

  try {
    const result = await fetch(import.meta.env.VITE_UPLOAD_URL, {
      method: 'POST',
      body: formData,
    }).then(res => res.json())
    loading.value = false
    return result
  }
  catch (e) {
    loading.value = false
  }
}

export function uploadFiles(files) {
  return Promise.all(files.map(file => uploadFile(file)))
}

export function createUploader() {
  return {
    uploadFile(file) {
      return uploadFile(file)
    },
    uploadFiles(files) {
      return uploadFiles(files)
    },
  }
}

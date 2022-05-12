export function useUploader() {
  const results = ref<FileResult[]>([])

  function setResult(result: FileResult) {
    const index = results.value.findIndex(x => x.id === result.id)
    results.value.splice(index, 1, result)
  }

  async function upload(file: Blob) {
    const result: FileResult = {
      id: Date.now(),
      url: URL.createObjectURL(file),
      code: 0,
      loading: true,
      message: '',
    }
    results.value = results.value.concat(result)

    const formData = new FormData()
    formData.append('file', file)
    formData.append('appId', import.meta.env.VITE_APP_ID)
    formData.append('appToken', import.meta.env.VITE_APP_TOKEN)
    try {
      const res = await fetch(import.meta.env.VITE_UPLOAD_URL, {
        method: 'POST',
        body: formData,
      }).then(res => res.json())
      setResult({ ...result, ...res, loading: false })
    }
    catch (e) {
      setResult({ ...result, code: 1, loading: false })
    }
  }

  return { upload, results }
}


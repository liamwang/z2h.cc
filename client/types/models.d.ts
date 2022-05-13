interface FileResult {
  id: number
  url: string
  code: number
  loading: boolean
  message: string
}

interface UploadResult {
  loading: boolean
  url: string | null
  code: number
  message: string
}

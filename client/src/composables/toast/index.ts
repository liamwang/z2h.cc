import { createVNode, render } from 'vue'
import ToastConstructor from './Toast.vue'

const defaultOptions = {
  horizontalPosition: 'center',
  verticalPosition: 'bottom',
  transition: 'slide-up',
  duration: 2500,
  message: '',
  closeable: false,
}

let seed = 1

function show(text: string, options = {}) {
  const id = `toast-${seed++}`
  const wrapper = document.createElement('div')
  wrapper.id = id
  const vnode = createVNode(
    ToastConstructor,
    { ...defaultOptions, ...options, message: text, id },
    null,
  )
  render(vnode, wrapper)
  document.body.appendChild(wrapper)
}

export default { show }

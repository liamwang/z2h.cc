<script lang="ts" setup>
const props = defineProps({
  id: { type: String, required: true },
  transition: { type: String, required: true },
  duration: { type: Number, required: true },
  message: { type: String, required: true },
})

const elRef = ref()

const state = reactive<any>({
  option: {},
  showing: false,
  timer: null,
})

const closed = ref(false)

const close = () => {
  closed.value = true
  state.timer = null
}

const startTimer = () => {
  if (props.duration > 0) {
    state.timer = setTimeout(() => {
      if (!closed.value)
        close()
    }, props.duration)
  }
}

onMounted(() => {
  startTimer()
  state.showing = true
})
onUnmounted(() => {
  clearTimeout(state.timer)
})

const removeHandler = () => {
  state.showing = false
  document.body.removeChild(document.getElementById(props.id)!)
  elRef.value.removeEventListener('transitionend', removeHandler, false)
}

watch(closed, (newVal) => {
  if (newVal) {
    state.showing = false
    elRef.value.addEventListener('transitionend', removeHandler, false)
  }
})
</script>

<template>
  <transition :name="transition">
    <div v-show="state.showing" ref="elRef" class="toast">
      <span v-html="message" />
    </div>
  </transition>
</template>

<style lang="less" scoped>
.toast {
  position: fixed;
  color: #fafcfe;
  background-color: #333;
  border-radius: 3px;
  padding: 4px 8px;
  max-width: 300px;
  z-index: 9999;
  box-shadow: 0 1px 2px rgba(150, 150, 150, 0.5);
}

// top-center
.toast {
  top: 10px;
  left: 50%;
  transform: translateX(-50%);
}

// todo: top-left top-right bottom-left bottom-center bottom-right

/* animations */

.slide-down-enter-active {
  transition: all 0.3s ease-out;
}

.slide-down-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}

.slide-down-enter-from,
.slide-down-leave-to {
  opacity: 0;
  bottom: -10% !important;
}

.slide-up-enter-active {
  transition: all 0.3s ease-out;
}

.slide-up-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}

.slide-up-enter-from,
.slide-up-leave-to {
  opacity: 0;
  top: -10% !important;
}

.fade-enter-active {
  transition: all 0.3s ease-out;
}

.fade-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>

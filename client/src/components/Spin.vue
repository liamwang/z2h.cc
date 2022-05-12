<script lang="ts" setup>

</script>

<template>
  <span class="spin i-ri-loader-4-line line-block">
    Loding...
  </span>
</template>

<style lang="less" scoped>
.spin {
  display: inline-block;
  animation: spinning 1s infinite linear;
}

@keyframes spinning {
  100% {
    transform: rotate(360deg);
  }
}
</style>

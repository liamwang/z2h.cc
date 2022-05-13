<script lang="ts" setup>

</script>

<template>
  <span class="spin i-ri-loader-4-line" />
</template>

<style lang="less" scoped>
.spin {
  display: inline-block;
  vertical-align: middle;
  animation: spinning 1s infinite linear;
}

@keyframes spinning {
  100% {
    transform: rotate(360deg);
  }
}
</style>

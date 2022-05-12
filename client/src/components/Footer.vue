<script setup lang="ts">
import { isDark, toggleDark } from '~/composables'
</script>

<template>
  <nav text-xl mt-6 inline-flex gap-2>
    <button class="icon-btn !outline-none" @click="toggleDark()">
      <div v-if="isDark" class="i-ri-moon-line" />
      <div v-else class="i-ri-sun-line" />
    </button>

    <a class="icon-btn" i-ri-github-fill rel="noreferrer" href="https://github.com/antfu/vitesse-lite" target="_blank"
      title="GitHub" />
  </nav>
</template>

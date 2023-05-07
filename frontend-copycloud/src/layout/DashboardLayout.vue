<template>
  <div  class="wrapper">
    <side-bar data-color = "black">
      <mobile-menu slot="content"></mobile-menu>
        <h4 style = "font:bold; color:white; margin:70px !important;">Workspace 1</h4>
      <br/>
      <br/>
      <br/>
      <sidebar-link to="/app">
        <p>Reports</p>
      </sidebar-link>
      <br/>
      <sidebar-link to="/explore">
        <p>Explore</p>
      </sidebar-link>

      
    </side-bar>
    <div class="main-panel">
      <top-navbar></top-navbar>

      <dashboard-content @click="toggleSidebar">

      </dashboard-content>

      <content-footer></content-footer>
    </div>
  </div>
</template>
<style lang="scss">

.wrapper {
  color:aliceblue !important;
}

</style>
<script>
  import TopNavbar from './TopNavbar.vue'
  import DashboardContent from './Content.vue'
  import MobileMenu from './MobileMenu.vue'
  export default {
    components: {
      TopNavbar,
      DashboardContent,
      MobileMenu
    },
    methods: {
      toggleSidebar () {
        if (this.$sidebar.showSidebar) {
          this.$sidebar.displaySidebar(false)
        }
      }
    }
  }

</script>

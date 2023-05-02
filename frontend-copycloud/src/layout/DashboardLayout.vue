<template>
  <div class="wrapper">
    <side-bar>
      <mobile-menu slot="content"></mobile-menu>
      <!-- <button class="button-9" role="button" >Create new project</button> -->
      <sidebar-link to="/app/myboard">
        <p>Projects</p>
      </sidebar-link>
      <sidebar-link to="/app/user">
        <p>User Profile</p>
      </sidebar-link>
      <!-- <sidebar-link to="/admin/table-list">
        <p>Table list</p>
      </sidebar-link> -->
      <sidebar-link to="/admin/typography">
        <p>Tools</p>
      </sidebar-link>
      <!-- <sidebar-link to="/admin/icons">
        <p>Icons</p>
      </sidebar-link>
      <sidebar-link to="/admin/maps">
        <p>Maps</p>
      </sidebar-link>
      <sidebar-link to="/admin/notifications">
        <p>Notifications</p>
      </sidebar-link> -->

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

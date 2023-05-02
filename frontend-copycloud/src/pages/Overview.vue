<template>
  <div class="content">
    <div class="container-fluid">  
      <div class="row">
        </div>
            <div class="row-md-1" style="right:100%">
              <div class = "col">
                <h2>My projects</h2>
              </div>
        </div>

        <div class="row">
          <div class = col-md-4>
            <stats-card  style="max-width: 600px; max-height: 600px;">
              <div slot="header" class="icon-success">
                <h4 class="card-title">New project</h4>
              </div>

              <div slot="content">
                <div class="button_plus" v-on:click="createProject()"></div>
              </div>
              <div slot = "footer">
              </div>
            </stats-card>
          </div>
          <div class = "col-md-4" v-for="(item,index) in items" :key="index" v-on:click="openProject(item.id)">
                <stats-card  class = "projectCard" style="max-width: 600px; max-height: 600px;">
                <div slot="header" class="icon-success">
                  <h4 class="card-title">{{ item.projectName }}</h4>
                </div>
                <div slot="content">
                  <p>{{ item.projectDescription }}</p>
                  <!-- <button v-on:click="openProject(item.listing_name)"></button> -->
                </div>
                <div slot = "footer">
                  <div class = "updateTime">
                    <i class="fa fa-history"></i> Updated 3 minutes ago
                  </div>
                  
                </div>
              </stats-card>
          </div>
        </div>

      </div>
    </div>
</template>
<script>
  import ChartCard from 'src/components/Cards/ChartCard.vue'
  import StatsCard from 'src/components/Cards/StatsCard.vue'
  import LTable from 'src/components/Table.vue'
  import axios from 'axios';
  export default {
    components: {
      LTable,
      ChartCard,
      StatsCard
    },
    data () {
      return {
        editTooltip: 'Edit Task',
        deleteTooltip: 'Remove',
        hovered: false,
        items: []
      }
    },
    async mounted(){
      await this.fetchAllProjects()
    },
    methods: {
      createProject(){
        console.log("Creating project")
      },
      async fetchAllProjects() {
        let data = []
        axios.get("http://localhost:5127/api/getallprojects",{
            withCredentials: true
            }).then(response => this.items = response.data)
              .catch(error => console.error(error));

      },
      openProject(selectedproject){
        console.log("Selected project")
        console.log(this.items)
        this.$router.push(`/app/project/${selectedproject}`)
      },

    }
  }
</script>
<style>
    .button-9{  padding: 1rem 3rem;
    text-align: center;
    font-size: 16px;
    text-transform: uppercase;
    cursor: pointer;
    background: linear-gradient(90deg, #288eec 0%, rgba(229, 20, 247, 0.63) 100%);
    border-radius: 53px;
    border: none;
    color: #fff;
    font-weight: bold;
    letter-spacing: 1px;}

    .button_plus {
    position: absolute;
    width: 35px;
    height: 35px;
    background: #fff;
    cursor: pointer;
    border: 2px solid #095776;

    /* Mittig */
    top: 50%;
    left: 50%;
  }

.button_plus:after {
  content: '';
  position: absolute;
  transform: translate(-50%, -50%);
  height: 4px;
  width: 50%;
  background: #095776;
  top: 50%;
  left: 50%;
}

.button_plus:before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: #095776;
  height: 50%;
  width: 4px;
}

.button_plus:hover:before,
.button_plus:hover:after {
  background: #fff;
  transition: 0.2s;
}

.button_plus:hover {
  background-color: #095776;
  transition: 0.2s;
}

;;;;

.projectCard:after {
  content: 'Open';
  position: absolute;
  transform: translate(-50%, -50%);
  height: 4px;
  width: 50%;
  background: #8bbabd;
  top: 50%;
  left: 50%;
}

.projectCard:before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  height: 100%;
  width: 100%;
}

.projectCard:hover:before,
.projectCard:hover:after {
  background: #6f7676;
  transition: 0.2s;
}

.projectCard:hover {
  background-color: #095776;
  transition: 0.2s;
}

.updateTime {
  color: black;
}




</style>

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
                <a class="button_plus" href="#project-create"></a>
              </div>

              <div slot = "footer">
              </div>
            </stats-card>
          </div>

          <div id="project-create" class="modal">
                <div class="modal__content">
                  <input v-model="projectTitle" placeholder = "Project title" class = "input_small"/>
                  <br/>
                  <input v-model="projectDescription" placeholder = "Project description" class = "input_small"/>
                    <br>
                    <div class="modal__footer">
                      <button class="button-9" role="button" v-on:click="createProject()">Create</button>
                    </div>

                      <a href="#" class="modal__close">&times;</a>
                  </div>
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
        items: [],
        projectTitle: "",
        projectDescription: "",
        createdProject: ""
      }
    },
    async mounted(){
      var baseUrl = window.location.origin;
        //window.location = baseUrl+`/app/project/`
      await this.fetchAllProjects()
    },
    methods: {
      createProject(){
        axios.post("http://localhost:5127/api/createproject",{
            withCredentials: true}, {"projectName":this.projectTitle, "projectDescription":this.projectDescription,"projectCreator":""})
            .then(response => this.createdProject = response.data)
            .catch(this.$router.push(`/app/myboard/`));
        var baseUrl = window.location.origin;
        window.location = baseUrl+`/app/project/${this.createdProject}`

      },
      async fetchAllProjects() {
        let data = []
        var baseUrl = window.location.origin;
        axios.get("http://localhost:5127/api/getallprojects",{
            withCredentials: true
            }).then(response => this.items = response.data)
              .catch(console.log(baseUrl));//this.$router.push(`/app/auth/`)

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

/* If you like this, be sure to ❤️ it. */
.wrapper-modal {
  height: 100vh;
  /* This part is important for centering the content */
  display: flex;
  align-items: center;
  justify-content: center;
  /* End center */
  background: -webkit-linear-gradient(to right, #834d9b, #d04ed6);
  background: linear-gradient(to right, #834d9b, #d04ed6);
}

.wrapper-modal a {
  display: inline-block;
  text-decoration: none;
  padding: 15px;
  background-color: #fff;
  border-radius: 3px;
  text-transform: uppercase;
  color: #585858;
  font-family: 'Roboto', sans-serif;
}

.modal {
  visibility: hidden;
  opacity: 0;
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(77, 77, 77, .7);
  transition: all .4s;
}

.modal:target {
  visibility: visible;
  opacity: 1;
}

.modal__content {
  border-radius: 4px;
  position: relative;
  width: 500px;
  max-width: 90%;
  background: #fff;
  padding: 1em 2em;
}

.modal__footer {
  text-align: right;
}

.modal__footer i {
  color: #d02d2c;
}

.modal__footer a {
  color: #585858;
}
.modal__close {
  position: absolute;
  top: 10px;
  right: 10px;
  color: #585858;
  text-decoration: none;
}

.input_small {
	 display: block;
	 border: none;
	 padding: 0;
	 width: 38ch;
	 background: repeating-linear-gradient(90deg, dimgrey 0, dimgrey 1ch, transparent 0, transparent 0ch) 0 100%/ 58ch 1px no-repeat;
	 font: 2.4ch droid sans mono, consolas, monospace;
	 letter-spacing: 0.15ch;
}

</style>

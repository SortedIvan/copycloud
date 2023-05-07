<template>
    <div style="height:100%; width: 100%; position:absolute; background-color: #302f2d;">
        <nav style = "background-color: white !important;padding:5px;margin:0;font-family:Arial, Tahoma, Serif; color:#263238; height:85px">
            <button v-on:click="goBack()" class="previous">&laquo; Go back</button>
            <h3>{{this.projectTitle}}</h3>
            <a class="btn2" href="#invite-to-project">Invite to project</a>

            <div id="invite-to-project" class="modal">
                <div class="modal__content" style = "background-color:#302f2d">
                    <h1 style = "color:white">Invite to project</h1>
                    <div class = "row">

                        <input v-model="inviteEmail" placeholder = "Invitee email" class = "input_small" style="margin:20px;"/>
                        <br/>


                        <div v-if="inviteSent">
                            <button class="button-9" role="button" style="color: gold;">Invite sent</button>
                        </div>
                        <div v-else>
                              <button class="button-9" role="button" v-on:click="inviteSent = true; sendInvite()">Send</button>
                        </div>
                    </div>

                    <div class="line"></div>
                    
                    <div class = "row">
                      <h4 style = "margin: 20px; color:white"> Create a shareable invite link:</h4>

                      <div v-if="projectInvite != ''">
                        <button  class="button-9" role="button" style="margin-left: 20px; color: gold;" v-on:click = "navigator.clipboard.writeText(this.projectInvite);">Invite copied</button>
                      </div>
                      <div v-else>
                        <button class="button-9" role="button" v-on:click="CreateProjectInvite()" style = "margin-left: 30px;">Create invite link</button>
                      </div>

                    </div>
                    <br>
                    <div class="modal__footer">
                        
                    </div>

                    <a href="#" class="modal__close">&times;</a>
                </div>
            </div>
        </nav>
        <div style = "height:auto">
            <div id="quillEditor" class = "quillCodEditor"></div>
        </div>
    </div>

</template>
<script>
import axios from 'axios';

export default {
    beforeMount(){

      window.addEventListener('keydown', (event) => {
            if (event.ctrlKey) {
              this.ctrlDown = true
              console.log(this.ctrlDown)
              event.preventDefault();
            }
          });
      window.addEventListener('keyup', (event) => {
        if (event.key === 's' && this.ctrlDown) {
          this.saveProject(this.quillJsEditor.getText());
          event.preventDefault();
        }
      });
      window.addEventListener('keyup', (event) => {
        if (event.ctrlKey) {
          this.ctrlDown = false;
          event.preventDefault();
        }
      });
    },
    async mounted() {
            // 1. Get the current logged in user 
            let email = await axios.get(`http://localhost:9000/api/getcurrentuser`,{
            withCredentials: true
            });

            // 2. Check if the project exists
            const path = this.$route.path.split('/');
            let projectId =  path[path.length - 1];
            
            this.currentProjectId = projectId;

            console.log(projectId)
            let projectExists = await axios.post("http://localhost:5127/api/checkprojectexists?projectId="+ projectId, { withCredentials:true});
            // 3. Check if user is in project
            let userInProject = false;
            const response = await axios.post('http://localhost:5127/api/checkuserinproject?projectId=' + projectId, {}, { withCredentials: true, headers: { Accept: 'text/plain' } });
            const { data } = response;
            userInProject = data;
            console.log(data ); // Do something with the response data
            // Handle the error here (e.g. show an error message to the user)

        
            console.log(projectExists.data + " projectExists")
            console.log(userInProject.data + " userInProject")
            console.log(email.data + " is email")

            this.currentUserEmail = email.data;

            if (projectExists.data && userInProject){
                let editor = new Quill("#quillEditor", {theme: 'snow'});

                this.quillJsEditor = editor;

                //Codox configuration
                let config = {
                  "app": "quilljs",
                  "editor": editor,
                  "docId": projectId,  //this is the unique id used to distinguish different documents 
                  "user": {"name": email.data}, //unqiue user name
                  "apiKey": "7e35dbaa-df13-4bd7-b7eb-9ab659233ee3" // this is your actual API Key
                };
                let codox = new Codox();

                //start coediting
                codox.start(config);
                this.editorReady = true;

                
                let documentContent = await axios.get("http://localhost:5127/api/getdocumentcontent?projectId="+projectId, { withCredentials: true})

                let projectDetails = await axios.get("http://localhost:5127/api/getprojectbyid?projectId="+projectId, {withCredentials: true});
                if (editor.getText() === "") {
                  editor.setText(documentContent.data + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                  return;
                }
                editor.setText(documentContent.data)
                
                this.projectTitle = projectDetails.data.projectName;
                let element = document.querySelector(".ql-toolbar");
                element.style.color = "white";
            }
            else {
                var baseUrl = window.location.origin;
                this.$router.push(baseUrl + `/admin/notfound`)
            }
    },
    name: 'CodoxEditor',
    contentChanges: [],
    components: {
    },
    data() {
    return { quillJsEditor: null,content: '', currentProjectId: "",currentUserEmail: "", editorReady: false, document, inviteSent:false, inviteEmail: "",projectTitle: "", ctrlDown: false, projectInvite: ""}
    },
    methods: {
        async sendInvite(){
            let invite = {
                    "invitee": this.inviteEmail,
                    "sender": this.currentUserEmail,
                    "projectId": this.currentProjectId
                }

            try {
              let invitedToProject = await axios.post("http://localhost:5127/api/invitetoproject", invite, { withCredentials:true});
              if (invitedToProject){
                this.inviteSent = true;
              }
            }
            catch {

            }
        },
        async saveProject(content) {

          let save = {
            "projectId": this.currentProjectId,
            "content": content
          }
          let response = await axios.post('http://localhost:5127/api/savedocument', save, { withCredentials: true, headers: { Accept: '*/*' } });
          console.log(response.data)
        },
        async CreateProjectInvite() {

          let invite = {
            "invitee": this.inviteEmail,
            "sender": this.currentUserEmail,
            "projectId": this.currentProjectId
          }
          
          try {
            let projectInviteResult = await axios.post("http://localhost:5127/api/createprojectinvite", invite, {withCredentials: true});
            this.projectInvite = projectInviteResult.data;
            navigator.clipboard.writeText(this.projectInvite);
          }
          catch {

          }
        },
        goBack(){
          var baseUrl = window.location.origin;
          window.location = baseUrl+`/app/`
        }
    },


};
</script>


<style>

.line::before {
    content: 'Or';
    position: absolute;
    top: 50%;
    left: 50%;
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
    background-color: #302f2d !important;
    padding: 0;
    color: white !important;
}
.quillCodEditor {
  color:white;
}

.quillCodEditor span {
  color:white !important;
}
.previous {
  background-color: #3d3e3f;
  border: none;
  border-radius: 3px;
  box-shadow: 0 -3px 0 rgba(0, 0, 0, 0.15) inset;
  color: #fff;
  letter-spacing: 0.1em;
  cursor: pointer;
  text-align: center;
  text-transform: uppercase;
  vertical-align: middle;
  white-space: nowrap;
  transition-property: transform, box-shadow;
  transform: translateZ(0);
  transition-duration: 0.5s;
  transition-timing-function: cubic-bezier(0.39, 0.5, 0.15, 1.36);
  font-family: "Futura PT", "Futura", sans-serif;
  height: 50px;
  width: 200px;
  margin:10px;
  display:flex;
  justify-content: center;
  align-items: center;
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

 input:focus {
	 outline: none;
	 color: dodgerblue;
}
 
.codox-avatar-initials{
    padding: 2px!important;;
    margin:4px!important;;
}
nav {
  display: flex; /* 1 */
  justify-content: space-between; /* 2 */
  padding: 1rem 1rem; /* 3 */
  background: #cfd8dc; /* 4 */
}

nav ul {
  display: flex; /* 5 */
  list-style: none; /* 6 */
}

nav li {
  padding-left: 1rem; /* 7! */
}
nav a {
  text-decoration: none;
  color: #0d47a1
}
/* 
  Extra small devices (phones, 600px and down) 
*/
@media only screen and (max-width: 600px) {
  nav {
    flex-direction: column;
  }
  nav ul {
    flex-direction: column;
    padding-top: 0.5rem;
  }
  nav li {
    padding: 0.5rem 0;
  }
} 
.ql-container .ql-snow {
    height: 100%;
}
.codox-styles .codox-container {
    overflow: visible;
    position: relative;
    background-color: transparent;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-pack: end;
    -ms-flex-pack: end;
    justify-content: left;
}

.codox-styles2 {
    display: flex;
    justify-content: flex-end;
}

.btn2,
.btn2:link,
.btn2:visited {
  background-color: #3d3e3f;
  border: none;
  border-radius: 3px;
  box-shadow: 0 -3px 0 rgba(0, 0, 0, 0.15) inset;
  color: #fff;
  letter-spacing: 0.1em;
  cursor: pointer;
  text-align: center;
  text-transform: uppercase;
  vertical-align: middle;
  white-space: nowrap;
  transition-property: transform, box-shadow;
  transform: translateZ(0);
  transition-duration: 0.5s;
  transition-timing-function: cubic-bezier(0.39, 0.5, 0.15, 1.36);
  font-family: "Futura PT", "Futura", sans-serif;
  height: 50px;
  width: 200px;
  margin:10px;
  display:flex;
  justify-content: center;
  align-items: center;
}

.btn2:hover,
.btn2:active {
box-shadow: 0 0 0 28px rgba(0, 0, 0, 0.25) inset;
}
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
/* If you like this, be sure to ❤️ it. */
.wrapper-modal {
  position:fixed !important;
  top: 100px !important; 
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
  position:fixed !important;
  top: 100px !important; 
  border-radius: 4px;
  position: relative;
  width: 500px;
  height: 350px;
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

.line{
    position: relative;
    height: 1px;
    width: 100%;
    margin: 36px 0;
    background-color: #d4d4d4;
}
.line::before{
    content: 'Or';
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background-color: #FFF;
    color: #8b8b8b;
    padding: 0 15px;
}
</style> 
  
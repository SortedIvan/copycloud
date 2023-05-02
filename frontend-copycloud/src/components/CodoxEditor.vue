<template>
    <div>
        <nav style = "padding:5px;margin:0;font-family:Arial, Tahoma, Serif; color:#263238; height:85px">
        <h3>Title</h3>

            <div class="wrapper-modal">
                <a class="btn2" href="#demo-modal">Invite to project</a>
            </div>

            <div id="demo-modal" class="modal">
                <div class="modal__content">
                    <h1>Invite to project</h1>

                    <p>
                        Please enter the invitee's email
                    </p>
                    <input v-model="inviteEmail" placeholder = "" class = "input_small"/>
                    
                    <br>
                    <div class="modal__footer">
                        <div v-if="inviteSent">
                          <button class="button-9" role="button" style="color: green;">Invite sent</button>
                        </div>
                        <div v-else>
                          <button class="button-9" role="button" v-on:click="inviteSent = true; sendInvite()">Send</button>
                        </div>
                    </div>

                    <a href="#" class="modal__close">&times;</a>
                </div>
            </div>
        </nav>
        <div style = "height:auto">
            <div id="quillEditor"></div>
        </div>
    </div>

</template>
<script>
import axios from 'axios';

export default {

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
                editor.getText()
                editor.setText("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n")

            }
            else {
                this.$router.push(`/admin/notfound`)
            }
    },
    name: 'CodoxEditor',
    contentChanges: [],
    components: {
    },
    data() {
    return { content: '', currentProjectId: "",currentUserEmail: "", editorReady: false, document, inviteSent:false, inviteEmail: "" }
    },
    methods: {
        async sendInvite(){
            let invite = {
                    "invitee": this.inviteEmail,
                    "sender": this.currentUserEmail,
                    "projectId": this.currentProjectId
                }
            let invitedToProject = await axios.post("http://localhost:5127/api/invitetoproject", invite, { withCredentials:true});

            if (invitedToProject){
              this.inviteSent = true;
            }
        }
    },

};
</script>


<style>
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
background-color: #a9c3ff;
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
height: 55px;
width: 200px;
margin:15px;
}

.btn2:hover,
.btn2:active {
box-shadow: 0 0 0 28px rgba(0, 0, 0, 0.25) inset;
}
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
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
</style> 
  
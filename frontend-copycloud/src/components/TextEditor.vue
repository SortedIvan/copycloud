<template>
  <vue-easymde class = "texteditorconfig" v-model="content" @keyup="postMark" ref="markdownEditor" />
 </template>
 

 <script>
  import VueEasymde from "vue-easymde";
  import Pusher from "pusher-js";
      
  export default {
    created() {
          let pusher = new Pusher("4976b018f55020dd0b34", {
            cluster: "eu",
            encrypted: true,
            authEndpoint: 'http://localhost:5127/api/authenticatepusher',
          });
          this.channel = pusher.subscribe("private-markdown");
          this.channel.bind("client-new-text", data => {
            this.content = data;
          });
          window.addEventListener('keydown', (event) => {
            if (event.ctrlKey && event.key === 's') {
              this.ctrlDown = true
              event.preventDefault();
            }
          });
          window.addEventListener('keyup', (event) => {
            // Check if the 'ctrl' key and the 's' key are pressed
            if (event.key === 's' && this.ctrlDown == true) {
              if (!this.saved){
                // Saving the document here
                console.log("saved")
                console.log(this.saved)
                this.saved = true;
                event.preventDefault();
              }
              event.preventDefault();
            }
            else if (event.ctrlKey) {
              this.ctrlDown = false;
            }
          });
        },
    name: 'TextEditor',
    contentChanges: [],
    components: {
      VueEasymde
    },
    data() {
    return { content: '', saved: false, ctrlDown: false}
    },
    methods: {
          postMark: function(e) {
              //this.saved = false;
              //this.channel.trigger("client-saved", false);
              //this.channel.trigger("client-new-text", this.content);
              console.log(this.content)
          }

    },

  };
 </script>
 
 
 <style>
  @import "~easymde/dist/easymde.min.css";
  .texteditorconfig {
    display:block
}
 </style> 

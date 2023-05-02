<template>
  <div class="content">
    <div class="container-fluid">
      <div class="row">
        <div class="col">
          <card>
            <template slot="header">
              <h1 class="card-title">📝 Call to action copy tool</h1>
              <br/>
              <h5>Generate copy based on action, goal and example input</h5>
            </template>
            
            <input v-model="email_receivers"  class = "input_title" placeholder = "CTO Copy title"/>

            <br/>
            <br/>

            <input v-model="actionGoal" placeholder = "Action: Subscribe to our newsletter, Join now, .." class = "input_small"/>
            <br/>
            <textarea v-model = "actionContext" placeholder = "Action context: We run a minimalistic & modern furniture ecom business" class="area1" cols="40" rows="5"></textarea>
            <br/>
            <button class="button-9" role="button" v-on:click="fetchData(amount, actionGoal, actionContext, tone, maxCharacter);">Generate</button>

          </card>
        </div>

      </div>
    </div>
  </div>
</template>
<script>
  import Card from 'src/components/Cards/Card.vue';
  import axios from 'axios';

  export default {
    components: {
      Card

    },
    data() {
        return{
            amount: 5,
            actionGoal: "Subscribe",
            actionContext: "",
            tone: "",
            maxCharacter: 500,
            success: false,
        };
    },
     methods: {
        showEmailVar() {
            console.log(this.email);
        },
        fetchData(amount, actionGoal, actionContext, tone, maxCharacter){
          let headers = { 
            'Access-Control-Allow-Origin' : true,
            'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
          }
          let url = "http://localhost:9001/api/getctocopies"
          axios.post(url,
          {
              "amount": amount,
              "actionGoal" : actionGoal,
              "actionContext": actionContext,
              "tone": tone,
              "maxCharacter": maxCharacter
          }, headers=headers)
          .then((response) => {
              this.success = true
              //self.createNotification(response.data)
              console.log(response.data);
              //this.$root.$emit('showNotificationEvent', 200)
          })
          .catch(error => {
            console.log(error)
            //this.$root.$emit('showNotificationEvent', 400)
          });
        }
      }
    }

</script>
<style>
.area1 {
	 display: block;
	 border: 5%;
	 padding: 0;
	 width: 33ch;
	 font: 3ch droid sans mono, consolas, monospace;
	 letter-spacing: 0.1ch;
}

.input_title {
	 display: block;
	 border: none;
	 padding: 0;
	 width: 33ch;
	 background: repeating-linear-gradient(90deg, dimgrey 0, dimgrey 0.5ch, transparent 0, transparent 1ch) 0 100%/ 38.7ch 2px no-repeat;
	 font: 4ch droid sans mono, consolas, monospace;
	 letter-spacing: 0.3ch;
}
.input_small {
	 display: block;
	 border: none;
	 padding: 0;
	 width: 58ch;
	 background: repeating-linear-gradient(90deg, dimgrey 0, dimgrey 1ch, transparent 0, transparent 0ch) 0 100%/ 58ch 1px no-repeat;
	 font: 2.4ch droid sans mono, consolas, monospace;
	 letter-spacing: 0.15ch;
}

 input:focus {
	 outline: none;
	 color: dodgerblue;
}
 

</style>

<template>
	<section class="container forms" :class="{ 'show-signup': isSignup }">
	  <div class="form login">
		<div class="form-content">
		  <div>Login</div>
		  <form>
			<div class="field-auth input-field-auth">
			  <input v-model="email" type="email" placeholder="Email" class="input" />
			</div>
			<div class="field-auth input-field-auth">
			  <input v-model="password" type="password" placeholder="Password" class="password" />
			  <i class='bx bx-hide eye-icon'></i>
			</div>
			<div v-if="passwordWrong">
				<p style="color:red">Wrong password, please try again.</p>
			</div>
			<br/>
			<div class="form-link">
			  <a href="#" class="forgot-pass">Forgot password?</a>
			</div>
			<div class="field-auth button-field-auth">
			  <button type = "button" v-on:click="login()">Login</button>
			</div>
		  </form>
		  <div class="form-link">
			<span>Don't have an account? <a href="#" class="link signup-link" @click.prevent="showSignup">Signup</a></span>
		  </div>
		</div>
		<div class="line"></div>
		<div class="media-options">
		  <a href="#" class="field-auth facebook">
			<i class='bx bxl-facebook facebook-icon'></i>
			<span>Login with Facebook</span>
		  </a>
		</div>
		<div class="media-options">
		  <a href="#" class="field-auth google">
			<img src="#" alt="" class="google-img" />
			<span>Login with Google</span>
		  </a>
		</div>
	  </div>
  
	  <div class="form signup">
		<div class="form-content">
		  <div>Signup</div>
		  <form>
			<div class="field-auth input-field-auth">
			  <input v-model="email" type="email" placeholder="Email" class="input" />
			</div>
			<div class="field-auth input-field-auth">
			  <input v-model="password" type="password" placeholder="Create password" class="password" />
			</div>
			<div class="field-auth input-field-auth">
			  <input v-model="confirmPassword" type="password" placeholder="Confirm password" class="password" />
			  <i class='bx bx-hide eye-icon'></i>
			</div>
			<div class="field-auth button-field-auth">
			  <button v-on:click="register()">Signup</button>
			</div>
		  </form>
		  <div class="form-link">
			<span>Already have an account? <a href="#" class="link login-link" @click.prevent="hideSignup">Login</a></span>
		  </div>
		</div>
		<div class="line"></div>
		<div class="media-options">
		  <a href="#" class="field-auth facebook">
			<i class='bx bxl-facebook facebook-icon'></i>
			<span>Login with Facebook</span>
		  </a>
		</div>
		<div class="media-options">
		  <a href="#" class="field-auth google">
			<img src="#" alt="" class="google-img" />
			<span>Login with Google</span>
		  </a>
		</div>
	  </div>
	</section>
  </template>
  
  <script>
  import axios from 'axios';
  export default {
	data() {
	  return {
		isSignup: false,
		email: "",
		password: "",
		confirmPassword: "",
		passwordWrong: false,
		success: false

	  };
	},
	methods: {
	  register(){
		let success = axios.post("http://localhost:9000/api/register", {
		"name": "test",
		"password": this.password,
		"confirmPassword": this.password,
		"email": this.email
		}, { withCredentials:true});
	  },
	  async login(){
			try{
			let loginCreds = {
				"email": this.email,
				"password": this.password
			}
			let success = await axios.post("http://localhost:9000/api/login", loginCreds, {headers: { Accept: '*/*' }, withCredentials: true});

			if (success.status != 200) {
				this.passwordWrong = true;
			} else {
				var baseUrl = window.location.origin;
				window.location = baseUrl + '/app'
			}
			}
			catch {
				console.log("error n")
				//this.$router.push({ path: '/auth' });
			}

		},
	  showSignup() {
		this.isSignup = true;
	  },
	  hideSignup() {
		this.isSignup = false;
	  },
	},
  };
  </script>
<style scoped>
 /* Google Fonts - Poppins */
 @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap');

.container{
    height: 100vh;
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    column-gap: 30px;
}
.form{
    position: absolute;
    max-width: 430px;
    width: 100%;
    padding: 30px;
    border-radius: 6px;
    background: #FFF;
}
.form.signup{
    opacity: 0;
    pointer-events: none;
}
.forms.show-signup .form.signup{
    opacity: 1;
    pointer-events: auto;
}
.forms.show-signup .form.login{
    opacity: 0;
    pointer-events: none;
}

form{
    margin-top: 30px;
}
.form .field-auth{
    position: relative;
    height: 50px;
    width: 100%;
    margin-top: 20px;
    border-radius: 6px;
}
.field-auth input,
.field-auth button{
    height: 100%;
    width: 100%;
    border: none;
    font-size: 16px;
    font-weight: 400;
    border-radius: 6px;
}
.field-auth input{
    outline: none;
    padding: 0 15px;
    border: 1px solid#CACACA;
}
.field-auth input:focus{
    border-bottom-width: 2px;
}
.eye-icon{
    position: absolute;
    top: 50%;
    right: 10px;
    transform: translateY(-50%);
    font-size: 18px;
    color: #8b8b8b;
    cursor: pointer;
    padding: 5px;
}
.field-auth button{
    color: #fff;
    background-color: #0171d3;
    transition: all 0.3s ease;
    cursor: pointer;
}
.field-auth button:hover{
    background-color: #016dcb;
}
.form-link{
    text-align: center;
    margin-top: 10px;
}
.form-link span,
.form-link a{
    font-size: 14px;
    font-weight: 400;
    color: #232836;
}

.form-content a:hover{
    text-decoration: underline;
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
.media-options a{
    display: flex;
    align-items: center;
    justify-content: center;
}
a.facebook{
    color: #fff;
    background-color: #4267b2;
}
a.facebook .facebook-icon{
    height: 28px;
    width: 28px;
    color: #0171d3;
    font-size: 20px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #fff;
}
.facebook-icon,
img.google-img{
    position: absolute;
    top: 50%;
    left: 15px;
    transform: translateY(-50%);
}
img.google-img{
    height: 20px;
    width: 20px;
    object-fit: cover;
}
a.google{
    border: 1px solid #CACACA;
}
a.google span{
    font-weight: 500;
    opacity: 0.6;
    color: #232836;
}

@media screen and (max-width: 400px) {
    .form{
        padding: 20px 10px;
    }
}
 
</style>
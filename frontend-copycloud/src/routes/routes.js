import DashboardLayout from '../layout/DashboardLayout.vue'
// GeneralViews
import NotFound from '../pages/NotFoundPage.vue'

// Admin pages
import Overview from 'src/pages/Overview.vue'
//import Project from 'src/pages/Project'
import CodoxEditor from 'src/components/CodoxEditor.vue'
import LoginRegister from 'src/components/LoginRegister.vue'
const routes = [
  {
    path: '/',
    component: DashboardLayout,
    redirect: '/app'
  },
  {
    path: '/app',
    name: 'MyBoard',
    component: Overview
  },
  {
    path: '/auth',
    name: 'Auth',
    component: LoginRegister
  },
  {
    path: '/project/*',
    name: 'Project',
    component: CodoxEditor
  },
  { path: '*', component: NotFound }
]

/**
 * Asynchronously load view (Webpack Lazy loading compatible)
 * The specified component must be inside the Views folder
 * @param  {string} name  the filename (basename) of the view to load.
function view(name) {
   var res= require('../components/Dashboard/Views/' + name + '.vue');
   return res;
};**/

export default routes

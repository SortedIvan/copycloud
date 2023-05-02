import DashboardLayout from '../layout/DashboardLayout.vue'
// GeneralViews
import NotFound from '../pages/NotFoundPage.vue'

// Admin pages
import Overview from 'src/pages/Overview.vue'
import UserProfile from 'src/pages/UserProfile.vue'
import Typography from 'src/pages/Typography.vue'
//import Project from 'src/pages/Project'
import CodoxEditor from 'src/components/CodoxEditor.vue'
import LoginRegister from 'src/components/LoginRegister.vue'
const routes = [
  {
    path: '/',
    component: DashboardLayout,
    redirect: '/app/myboard'
  },
  {
    path: '/app',
    component: DashboardLayout,
    redirect: '/app/myboard',
    children: [
      {
        path: 'myboard',
        name: 'Overview',
        component: Overview
      },
      {
        path: 'user',
        name: 'User',
        component: UserProfile
      },
      {
        path: 'typography',
        name: 'Typography',
        component: Typography
      },
      {
        path: 'project/*',
        name: 'Project',
        component: CodoxEditor
      },
      {
        path: 'test',
        name: 'test',
        component: CodoxEditor
      },
      {
        path: 'auth',
        name: 'Auth',
        component: LoginRegister
      }

    ]
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

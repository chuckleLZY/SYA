import Vue from 'vue'
import VueRouter from 'vue-router'

import Main from '../components/Main.vue'

import LogIn from '../views/Login.vue'

import Home from '../views/Home.vue'
import TeacherTable from '../components/TeacherTable.vue'
import AdminTable from '../components/AdminTable.vue'
import StudentMessage from '../components/StudentMessage.vue'
import TeacherMessage from '../components/TeacherMessage.vue'
import AdminMessage from '../components/AdminMessage.vue'
import StudentMine from '../components/StudentMine.vue'
import TeacherMine from '../components/TeacherMine.vue'
import AdminMine from '../components/AdminMine.vue'

import Recruitment from '../views/Student/Recruitment.vue'
import StudentFavorites from '../views/Student/StudentFavorites.vue'
import TeacherFavorites from '../views/Teacher/TeacherFavorites.vue'
import StudentSystemMessage from '../views/Student/StudentSystemMessage.vue'
import StudentWorkMessage from '../views/Student/StudentWorkMessage.vue'

import StudentInformation from '../views/Student/StudentInformation.vue'
import StudentResume from '../views/Student/StudentResume.vue'
import StudentWorkManagement from '../views/Student/StudentWorkManagement.vue'

import ReceivedResume from '../views/Teacher/ReceivedResume.vue'
import PublishWorkMessage from '../views/Teacher/PublishWorkMessage.vue'
import MyPublishedWork from '../views/Teacher/MyPublishedWork.vue'

import TeacherSystemMessage from '../views/Teacher/TeacherSystemMessage.vue'
import TeacherWorkMessage from '../views/Teacher/TeacherWorkMessage.vue'

import TeacherInformation from '../views/Teacher/TeacherInformation.vue'
import TeacherQualification from '../views/Teacher/TeacherQualification.vue'


import PublishSystemMessage from '../views/Admin/PublishSystemMessage.vue'
import AuditWork from '../views/Admin/AuditWork.vue'


import AdminSystemMessage from '../views/Admin/AdminSystemMessage.vue'

import AdminInformation from '../views/Admin/AdminInformation.vue'
import AdminQualification from '../views/Admin/AdminQualification.vue'




Vue.use(VueRouter)

const routes = [
  // {
  //   path: '/',
  //   name: 'Home',
  //   component: Home
  // },
  // {
  //   path: '/about',
  //   name: 'About',
  //   // route level code-splitting
  //   // this generates a separate chunk (about.[hash].js) for this route
  //   // which is lazy-loaded when the route is visited.
  //   component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  // }
  {
    path: '/',
    name: 'LogIn',
    // redirect:'/LogIn',
    component: LogIn,
  },
  {
    path: '/Main',
    name: 'Main',
    component: Main,
    redirect:'/Home',
    children:[
      {
        path: '/Home',
        name: 'Home',
        component: Home
      },
      {
        path: '/Recruitment',
        name: 'Recruitment',
        component: Recruitment
      },
      {
        path: '/TeacherTable',
        name: 'TeacherTable',
        component: TeacherTable,
        children: [
          {
            path: '/ReceivedResume',
            name: 'ReceivedResume',
            component: ReceivedResume,
          },
          {
            path: '/PublishWorkMessage',
            name: 'PublishWorkMessage',
            component: PublishWorkMessage,
          },
          {
            path: '/MyPublishedWork',
            name: 'MyPublishedWork',
            component: MyPublishedWork,
          }
        ]
      },
      {
        path: '/AdminTable',
        name: 'AdminTable',
        component: AdminTable,
        children: [
          {
            path: '/PublishSystemMessage',
            name: 'PublishSystemMessage',
            component: PublishSystemMessage,
          },
          {
            path: '/AuditWork',
            name: 'AuditWork',
            component: AuditWork,
          }
        ]
      },
      {
        path: '/StudentMessage',
        name: 'StudentMessage',
        component: StudentMessage,
        children: [
          {
            path: '/StudentSystemMessage',
            name: 'StudentSystemMessage',
            component: StudentSystemMessage,
          },
          {
            path: '/StudentWorkMessage',
            name: 'StudentWorkMessage',
            component: StudentWorkMessage,
          }
        ]
      },
      {
        path: '/TeacherMessage',
        name: 'TeacherMessage',
        component: TeacherMessage,
        children: [
          {
            path: '/TeacherSystemMessage',
            name: 'TeacherSystemMessage',
            component: TeacherSystemMessage,
          },
          {
            path: '/TeacherWorkMessage',
            name: 'TeacherWorkMessage',
            component: TeacherWorkMessage,
          }
        ]
      },
      {
        path: '/AdminMessage',
        name: 'AdminMessage',
        component: AdminMessage,
        children: [
          {
            path: '/AdminSystemMessage',
            name: 'AdminSystemMessage',
            component: AdminSystemMessage,
          },
        ]
      },
      {
        path: '/StudentMine',
        name: 'StudentMine',
        component: StudentMine,
        children: [
          {
            path: '/StudentInformation',
            name: 'StudentInformation',
            component: StudentInformation,
          },
          {
            path: '/StudentResume',
            name: 'StudentResume',
            component: StudentResume,
          },
          {
            path: '/StudentWorkManagement',
            name: 'StudentWorkManagement',
            component: StudentWorkManagement,
          }
        ]
      },
      {
        path: '/TeacherMine',
        name: 'TeacherMine',
        component: TeacherMine,
        children: [
          {
            path: '/TeacherInformation',
            name: 'TeacherInformation',
            component: TeacherInformation,
          },
          {
            path: '/TeacherQualification',
            name: 'TeacherQualification',
            component: TeacherQualification,
          }
        ]
      },
      {
        path: '/AdminMine',
        name: 'AdminMine',
        component: AdminMine,
        children: [
          {
            path: '/AdminInformation',
            name: 'AdminInformation',
            component: AdminInformation,
          },
          {
            path: '/AdminQualification',
            name: 'AdminQualification',
            component: AdminQualification,
          }
        ]
      },
      {
        path: '/StudentFavorites',
        name: 'StudentFavorites',
        component: StudentFavorites
      },
      {
        path: '/TeacherFavorites',
        name: 'TeacherFavorites',
        component: TeacherFavorites
      },
    
    ]
  },
  
]

const router = new VueRouter({
  routes
})

export default router

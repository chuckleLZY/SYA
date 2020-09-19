import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        redirect: '/LogIn',
    },
    {
        path: '/LogIn',
        name: 'LogIn',
        component: () => import('../views/Login.vue'),
    },
    {
        path: '/Register',
        name: 'Register',
        component: () => import('../views/Register.vue'),
    },
    {
        path: '/Main',
        name: 'Main',
        component: () => import('../components/Main.vue'),
        redirect: '/Home',
        children: [{
            path: '/Home',
            name: 'Home',
            component: () => import('../views/Home.vue'),
        },
        {
            path: '/Recruitment',
            name: 'Recruitment',
            component: () => import('../views/Student/Recruitment.vue'),
        },
        {
            path: '/TeacherTable',
            name: 'TeacherTable',
            component: () => import('../components/TeacherTable.vue'),
            redirect: '/ReceivedResume',
            children: [{
                path: '/ReceivedResume',
                name: 'ReceivedResume',
                component: () => import('../views/Teacher/ReceivedResume.vue'),
            },
            {
                path: '/PublishWorkMessage',
                name: 'PublishWorkMessage',
                component: () => import('../views/Teacher/PublishWorkMessage.vue'),
            },
            {
                path: '/MyPublishedWork',
                name: 'MyPublishedWork',
                component: () => import('../views/Teacher/MyPublishedWork.vue'),
            }
            ]
        },
        {
            path: '/AdminTable',
            name: 'AdminTable',
            component: () => import('../components/AdminTable.vue'),
            children: [{
                path: '/PublishSystemMessage',
                name: 'PublishSystemMessage',
                component: () => import('../views/Admin/PublishSystemMessage.vue'),
            },
            {
                path: '/AuditWork',
                name: 'AuditWork',
                component: () => import('../views/Admin/AuditWork.vue'),
            }
            ]
        },
        {
            path: '/StudentMessage',
            name: 'StudentMessage',
            component: () => import('../components/StudentMessage.vue'),
            redirect: '/StudentSystemMessage',
            children: [{
                path: '/StudentSystemMessage',
                name: 'StudentSystemMessage',
                component: () => import('../views/Student/StudentSystemMessage.vue'),
            },
            {
                path: '/StudentWorkMessage',
                name: 'StudentWorkMessage',
                component: () => import('../views/Student/StudentWorkMessage.vue'),
            }
            ]
        },
        {
            path: '/TeacherMessage',
            name: 'TeacherMessage',
            component: () => import('../components/TeacherMessage.vue'),
            redirect: '/TeacherSystemMessage',
            children: [{
                path: '/TeacherSystemMessage',
                name: 'TeacherSystemMessage',
                component: () => import('../views/Teacher/TeacherSystemMessage.vue'),
            },
            {
                path: '/TeacherWorkMessage',
                name: 'TeacherWorkMessage',
                component: () => import('../views/Teacher/TeacherWorkMessage.vue'),
            },
            {
                path: '/TeacherResignMessage',
                name: 'TeacherResignMessage',
                component: () => import('../views/Teacher/TeacherResignMessage.vue'),
            }
            ]
        },
        {
            path: '/AdminMessage',
            name: 'AdminMessage',
            component: () => import('../components/AdminMessage.vue'),
            children: [{
                path: '/AdminSystemMessage',
                name: 'AdminSystemMessage',
                component: () => import('../views/Admin/AdminSystemMessage.vue'),
            },]
        },
        {
            path: '/StudentMine',
            name: 'StudentMine',
            component: () => import('../components/StudentMine.vue'),
            redirect: '/StudentInformation',
            children: [{
                path: '/StudentInformation',
                name: 'StudentInformation',
                component: () => import('../views/Student/StudentInformation.vue'),
            },
            {
                path: '/StudentResume',
                name: 'StudentResume',
                component: () => import('../views/Student/StudentResume.vue'),
            },
            {
                path: '/StudentWorkManagement',
                name: 'StudentWorkManagement',
                component: () => import('../views/Student/StudentWorkManagement.vue'),
            },
            {
                path: '/StudentLeaveManagement',
                name: 'StudentLeaveManagement',
                component: () => import('../views/Student/StudentLeaveManagement.vue'),
            },
            ]
        },
        {
            path: '/TeacherMine',
            name: 'TeacherMine',
            component: () => import('../components/TeacherMine.vue'),
            redirect: '/TeacherInformation',
            children: [{
                path: '/TeacherInformation',
                name: 'TeacherInformation',
                component: () => import('../views/Teacher/TeacherInformation.vue'),
            },
            ]
        },
        {
            path: '/StudentFavorites',
            name: 'StudentFavorites',
            component: () => import('../views/Student/StudentFavorites.vue'),
        },
        {
            path: '/TeacherFavorites',
            name: 'TeacherFavorites',
            component: () => import('../views/Teacher/TeacherFavorites.vue'),
        }
        ]
    },

]



const router = new VueRouter({
    mode: 'history',
    routes
});

export default router;

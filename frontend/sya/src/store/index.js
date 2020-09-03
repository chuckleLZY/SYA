import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        isLoggedIn: false,
        role: -1,
    },
    mutations:{
        logIn(state,role){
            state.isLoggedIn=true;
            state.role=role;
        }
    }
});

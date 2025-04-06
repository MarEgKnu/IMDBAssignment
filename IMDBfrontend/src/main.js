import { h } from 'vue'
import { createApp } from 'vue/dist/vue.esm-bundler';

import 'axios'
import axios from 'axios'

class Person {
    constructor(id, nConst, primaryName, birthYear, deathYear, roles, titles) {
        this.id = id; 
        this.nConst = nConst; 
        this.primaryName = primaryName; 
        this.birthYear = birthYear, 
        this.deathYear = deathYear; 
        this.roles = roles; 
        this.titles = titles; 
    }
}

const basePersonURL = "http://localhost:5073/Persons/Persons";

let app = createApp({
    data() {
        return {
            personSearch: "",
            personOffset: 0,
            personAscending: true,
            someNum: 1,
            persons: [],
        }
    },

    methods: {
        async GetPersons() {
            
            const response = await axios.get(`${basePersonURL}?search=${this.personSearch}&offset=${this.personOffset}&rows${this.personRows}&ascending=${this.personAscending}`).then(
                (response) => {
                    const personData = []; 
                    response.data.forEach(element => {
                        let p = new Person(element.id, element.nConst, element.primaryName, element.birthYear ?? "Ukendt", element.deathYear ?? "Ukendt", element.roles ?? "Ukendt", element.titles ?? "Ukendt");
                        personData.push(p);
                        console.log(p); 
                    });
                    this.persons = personData; 
                    

                }
                
            )
        }, 

        Previous50(){
            if (this.personOffset > 0){
                this.personOffset -= 50; 
            }
            this.GetPersons(); 
        },

        Next50(){
            this.personOffset += 50; 
            this.GetPersons(); 
        },


        
    },

    mounted() {
        console.log(this.persons.length); 
    },




}); 
app.mount("#app"); 




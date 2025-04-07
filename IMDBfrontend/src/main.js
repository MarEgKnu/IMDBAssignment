import { h, onMounted, toRaw } from 'vue'

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

class Title {
    constructor(id, tConst, titleType, primaryTitle, originalTitle, isAdult, startYear, endYear, runTimeMinutes, aggregatedGenres) {
        this.id = id;
        this.tConst = tConst;
        this.titleType = titleType;
        this.primaryTitle = primaryTitle ?? "Ukendt";
        this.originalTitle = originalTitle ?? "Ukendt";
        this.isAdult = isAdult ?? "Ukendt";
        this.startYear = startYear ?? "Ukendt";
        this.endYear = endYear ?? "Ukendt";
        this.runTimeMinutes = runTimeMinutes ?? "Ukendt";
        this.aggregatedGenres = aggregatedGenres ?? "Ukendt";
    }
}

const basePersonURL = "http://localhost:5073/Persons/Persons";
const baseTitleURL = "http://localhost:5073/Titles/Titles";
const baseGenresURL = "http://localhost:5073/Genres";

let app = createApp({
    data() {
        return {
            isLoading: false,

            acceptedGenres: [],
            personSearch: "",
            personOffset: 0,
            personAscending: true,
            personsResponseMessage: null,
            persons: [],

            titleSearch: "",
            titleOffset: 0,
            titleAscending: true,
            titles: [],


            nConst: null,
            primaryName: null,
            birthYear: null,
            deathYear: null,
            roles: null,
            personTitles: null,

            errorMessage: null,
            responseMessage: null,
            titleType: null,

            putID: null,
            tConst: null,
            titleType: null,
            primaryTitle: null,
            originalTitle: null,
            isAdult: false,
            startYear: null,
            endYear: null,
            runTimeMinutes: null,
            genres: null,

            titleErrorMessage: null,
            titleResponseMessage: null,

            deleteID: null,
            titleDELETEErrorMessage: null,
            titleDELETEResponseMessage: null,
        }
    },

    methods: {
        async GetPersons() {
            this.ResetMessages();
            this.isLoading = true;

            const response = await axios.get(`${basePersonURL}?search=${this.personSearch}&offset=${this.personOffset}&ascending=${this.personAscending}`).then(
                (response) => {
                    const personData = [];
                    response.data.forEach(element => {
                        let p = new Person(element.id, element.nConst, element.primaryName, element.birthYear ?? "Ukendt", element.deathYear ?? "Ukendt", element.roles ?? "Ukendt", element.titles ?? "Ukendt");
                        personData.push(p);

                    });
                    this.persons = personData;

                    this.personsResponseMessage = `${this.persons.length} resultater fundet`;

                }

            )
            this.isLoading = false;
        },

        Previous50() {
            if (this.personOffset > 0) {
                this.personOffset -= 50;
            }
            this.GetPersons();
        },

        Next50() {
            this.personOffset += 50;
            this.GetPersons();
        },

        ClearResults() {
            this.persons = [];
            this.titles = [];
            this.ResetMessages();
        },

        async GetTitles() {
            this.ResetMessages();
            this.isLoading = true;
            const response = await axios.get(`${baseTitleURL}?search=${this.titleSearch}&offset=${this.titleOffset}&ascending=${this.titleAscending}`).then(
                (response) => {
                    const titleData = [];
                    response.data.forEach(element => {
                        let t = new Title(element.id, element.tConst, element.titleType, element.primaryTitle, element.originalTitle, element.isAdult, element.startYear, element.endYear, element.runTimeMinutes, element.aggregatedGenres);
                        titleData.push(t);


                    });
                    this.titles = titleData;
                    this.titleResponseMessage = `${this.titles.length} resultater fundet`;

                }


            )
            this.isLoading = false;
        },

        GetString(stringArr) {
            let res = toRaw(stringArr);
            let arr = [];
            for (const key in res) {
                if (Object.prototype.hasOwnProperty.call(res, key)) {
                    const element = res[key];
                    arr.push(element);
                }
            }

            let string = arr.join(", "); 
            return string;
        },

        async POSTPerson() {
            this.ResetMessages();

            if (this.nConst == "" || this.nConst == "" || this.primaryName == "" || this.primaryName == null) {
                this.errorMessage = "Du mangler at indtaste data";
                return;
            }
            if (this.roles != null) {
                let someRoles = toRaw(this.roles);
                var stringArr = someRoles.split(", ");

            }
            if (this.personTitles != null) {
                var someTitles = this.personTitles.split(", ");

            }
            let person = new Person(0, this.nConst, this.primaryName, this.birthYear, this.deathYear, stringArr, someTitles);
            const response = await axios.post(basePersonURL, person).then((response) => {
                console.log(response);
                this.responseMessage = "Person indsat!"
            }).catch((error) => {
                this.errorMessage = "Husk kun at skrive komma-separerede ID'er i titles samt roller er actor, writer etc.";
            });

            ;
        },

        async POSTTitle() {
            this.ResetMessages();

            if (this.tConst == "" || this.tConst == "" || this.titleType == null || this.titleType == "") {
                this.titleErrorMessage = "Du mangler at indtaste data, enten tConst eller titleType";
                return;
            }
            if (this.genres != null) {
                let someGenres = toRaw(this.genres);
                var stringArr = someGenres.split(", ");

            }
            let title = { tconst: this.tConst, titleType: this.titleType, primaryTitle: this.primaryTitle, originalTitle: this.originalTitle, isAdult: this.isAdult, startYear: this.startYear, endYear: this.endYear, runTimeMinutes: this.runTimeMinutes, Genres: stringArr ?? null };
            const response = await axios.post(baseTitleURL, title).then((response) => {
                console.log(response);
                this.titleResponseMessage = "Titel indsat!"
            }).catch((error) => {
                this.titleErrorMessage = "Husk kun at skrive komma-separerede værdier i genrer";
                console.log(error.data)
            });
            console.log(title);



        },

        async DELETETitle() {
            this.ResetMessages();

            if (this.deleteID == undefined) {
                this.titleDELETEErrorMessage = "Du mangler at indtaste ID";
                return;
            }

            const response = await axios.delete(`${baseTitleURL}?id=${this.deleteID}`).then((response) => {
                console.log(response.status);

                this.titleDELETEResponseMessage = "Titel slettet!"
            }).catch((error) => {
                this.titleDELETEErrorMessage = "Husk kun at skrive komma-separerede værdier i genrer";
                console.log(error.data)
            });




        },

        async PUTTitle() {
            this.ResetMessages();
            if (this.putID == null || this.tConst == "" || this.tConst == "" || this.titleType == null || this.titleType == "") {
                this.titleErrorMessage = "Du mangler at indtaste data, enten tConst eller titleType eller ID";
                return;
            }
            if (this.genres != null) {
                let someGenres = toRaw(this.genres);
                var stringArr = someGenres.split(", ");
                for (let index = 0; index < stringArr.length; index++) {
                    const element = stringArr[index];
                    if (!this.acceptedGenres.includes(element)) {
                        this.titleErrorMessage = "Du har skrevet genrer, som ikke er godkendt - kig evt. i konsol for accepterede genrer";
                        console.log(this.acceptedGenres);
                        return;
                    }
                }

            }
            let title = { tconst: this.tConst, titleType: this.titleType, primaryTitle: this.primaryTitle, originalTitle: this.originalTitle, isAdult: this.isAdult, startYear: this.startYear, endYear: this.endYear, runTimeMinutes: this.runTimeMinutes, Genres: stringArr ?? null };
            const response = await axios.put(`${baseTitleURL}?id=${this.putID}`, title).then((response) => {
                console.log(response);
                this.titleResponseMessage = "Titel opdateret!"
            }).catch((error) => {
                this.titleErrorMessage = "Husk kun at skrive komma-separerede værdier i genrer";
                console.log(error.data)
            });
            console.log(title);



        },

        async GetGenres() {
            let arr = [];
            const response = await axios.get(baseGenresURL).then((response) => {
                let data = response.data;

                data.forEach(elem => {
                    arr.push(elem.name);
                });

            });
            this.acceptedGenres = arr;
        },



        ResetMessages() {
            this.errorMessage = null;
            this.titleErrorMessage = null;
            this.titleDELETEErrorMessage = null;
            this.responseMessage = null;
            this.titleResponseMessage = null;
            this.personsResponseMessage = null;
            this.titleDELETEResponseMessage = null;
        }




    },

    computed: {
        none() {
            return this.persons.length != 0 || this.titles.length != 0 ? "btn-success" : "invisible";
        },

        show() {
            return this.persons.length != 0 || this.titles.length != 0 ? "btn-info" : "invisible";
        },

        showTitleButtons() {
            return this.titles.length != 0 ? "btn-info" : "invisible";
        },

        loading() {
            return this.isLoading;
        }
    },

    async mounted() {
        await this.GetGenres();

        console.log(this.acceptedGenres)


    },




});
app.mount("#app");




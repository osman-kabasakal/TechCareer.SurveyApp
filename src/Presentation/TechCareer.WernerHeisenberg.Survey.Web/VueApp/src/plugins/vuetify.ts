// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// Composables
import {createVuetify} from 'vuetify'

export default createVuetify({
  theme: {
    themes: {
      light: {
        colors: {
          primary: '#009688',
          secondary: '#ff9800',
          accent: '#3f51b5',
          error: '#f44336',
          warning: '#ffeb3b',
          info: '#00bcd4',
          success: '#4caf50'
        },
      },
    },
  },
})

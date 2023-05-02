<template>
    <div class="content">
        <div class="container-fluid">
            <TextEditor/>
        </div>
    </div>  
</template>
<style scoped>

</style>
<script>
import TextEditor from '../components/TextEditor.vue'
export default {
    name: "Project",
    components: {
        TextEditor
    },
    data () {
      return {
        editTooltip: 'Edit Task',
        deleteTooltip: 'Remove',
        hovered: false,
        
        projectName: "",
        projectUsers: [],
        projectContent: ""
      }
    }
    
}
</script>

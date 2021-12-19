using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Nome é Obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este nome deve ter entre 3 e 40 caracteres")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Slug é Obrigatório")]
        public string Slug { get; set; }

    }
}

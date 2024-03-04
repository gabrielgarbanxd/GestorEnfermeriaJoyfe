using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.VisitTemplatePersistence
{
    public class MySqlVisitTemplateRepository : MySqlRepositoryBase<VisitTemplate>, IVisitTemplateContract
    {
        public MySqlVisitTemplateRepository(VisitTemplateMapper mapper) : base(mapper)
        {
        }

        public MySqlVisitTemplateRepository()
        {
        }

        public async Task<IEnumerable<VisitTemplate>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"p_per_page", perPage},
                    {"p_page", page}
                };

                return await ExecuteQueryAsync("GetAllVisitsPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllVisitsProcedure");
        }

        public async Task<VisitTemplate> FindAsync(VisitTemplateId visitTemplateId)
        {
            var result = await ExecuteQueryAsync("GetVisitByIdProcedure", new Dictionary<string, object> { { "p_id", visitTemplateId.Value } });

            if (!result.Any())
            {
                throw new System.Exception("No se ha encontrado la visita.");
            }

            return result.First();
        }

        public async Task<int> CreateAsync(VisitTemplate visitTemplate)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_name", visitTemplate.Name.Value},
                {"p_type", visitTemplate.Type.Value},
                {"p_classification", visitTemplate.Classification.Value},
                {"p_is_comunicated",visitTemplate.IsComunicated.ToInt()},
                {"p_is_derived",visitTemplate.IsDerived.ToInt()},
                {"p_trauma_type",visitTemplate.TraumaType?.Value ?? ""},
                {"p_place",visitTemplate.Place?.Value ?? ""}
            };

            return await ExecuteNonQueryAsync("CreateVisitProcedure", parameters);
        }

        public async Task<int> UpdateAsync(VisitTemplate visitTemplate)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", visitTemplate.Id.Value},
                {"p_name", visitTemplate.Name.Value},
                {"p_type", visitTemplate.Type.Value},
                {"p_classification", visitTemplate.Classification.Value},
                {"p_is_comunicated",visitTemplate.IsComunicated.ToInt()},
                {"p_is_derived",visitTemplate.IsDerived.ToInt()},
                {"p_trauma_type",visitTemplate.TraumaType?.Value ?? ""},
                {"p_place",visitTemplate.Place?.Value ?? ""}
            };

            return await ExecuteNonQueryAsync("UpdateVisitProcedure", parameters);
        }

        public async Task<int> DeleteAsync(VisitTemplateId visitTemplateId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", visitTemplateId.Value}
            };

            return await ExecuteNonQueryAsync("DeleteVisitProcedure", parameters);
        }
    }
}

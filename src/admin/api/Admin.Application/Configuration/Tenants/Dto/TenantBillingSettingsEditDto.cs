namespace Magicodes.Admin.Configuration.Tenants.Dto
{
    public class TenantBillingSettingsEditDto
    {
        /// <summary>
        /// ̧ͷ����
        /// </summary>
        public string LegalName { get; set; }
        /// <summary>
        /// ��ַ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// ˰��
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// ��ϵ��ʽ
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// �����˻�
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public string Bank { get; set; }
    }
}
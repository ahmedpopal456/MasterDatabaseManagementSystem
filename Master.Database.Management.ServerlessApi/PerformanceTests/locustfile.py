from locust import HttpUser, task, between, tag
import uuid

class FixitFix(HttpUser):
    host = "http://localhost:7071"
    wait_time = between(2, 10)

    @tag('get_fix_template', 'fix')
    @task(5)
    def get_fix_template(self):
        self.client.get("/api/fixTemplates/327f43e3-44b5-45b4-9d0d-7ae2e3cefe76",
        name="get_fix_template")

    @tag('update_system_cost_estimate', 'fix')
    @task(2)
    def update_system_cost_estimate(self):
        self.client.put("/api/fixTemplates/327f43e3-44b5-45b4-9d0d-7ae2e3cefe76/cost/100.99",
        name="update_system_cost_estimate")

    @tag('update_fix_template_status', 'fix')
    @task(2)
    def update_fix_template_status(self):
        self.client.put("/api/fixTemplates/327f43e3-44b5-45b4-9d0d-7ae2e3cefe76/status/Public",
        name="update_fix_template_status")


    @tag('update_fix_template_1', 'fix')
    @task(2)
    def update_fix_template_1(self):
        self.client.put("/api/fixTemplates/388e93e4-2bfc-42df-b251-8eb0cd9b7204",
        json={
            "Name": "Template Name",
            "CategoryId": "ee7b2f94-49f3-4628-aca4-94b0d7cf2b3a",
            "TypeId": "445e50d1-b2e7-4c25-a628-c610aed7a357",
            "Description": "It worked!!",
            "UpdatedByUserId": "0ba10288-fa9d-420c-bae3-b5326f773130",
            "Tags": ["Tag1", "Tag2", "Tag3"],
            "Sections": [
                {
                    "Name": "Section Name 3",
                    "Fields": [
                        {
                            "Name": "Field Name 3.1",
                            "Values": ["Value 3.1.1", "Value 3.1.2"]
                        },
                        {
                            "Name": "Field Name 3.2",
                            "Values": ["Value 3.2.1", "Value 3.2.2"]
                        }
                    ]
                },
                {
                    "Name": "Section Name 2",
                    "Fields": [
                        {
                            "Name": "Field Name 2.1",
                            "Values": ["Value 2.1.1"]
                        },
                        {
                            "Name": "Field Name 2.2",
                            "Values": ["Value 2.2.1", "Value 2.2.2"]
                        }
                    ]
                }
            ]
        },
        name="update_fix_template_1")

    
    @tag('get_fix_template_by_userId', 'fix')
    @task(5)
    def get_fix_template_by_userId(self):
        self.client.get("/api/fixTemplates/users/0ba10288-fa9d-420c-bae3-b5326f773130",
        name="get_fix_template_by_userId")

    @tag('get_paged_fix_template_by_userId', 'fix')
    @task(5)
    def get_paged_fix_template_by_userId(self):
        self.client.get("/api/fixTemplates/users/0ba10288-fa9d-420c-bae3-b5326f773130/1?pageSize=20&status=Private",
        name="get_paged_fix_template_by_userId")

    @tag('get_categories', 'fix')
    @task(5)
    def get_categories(self):
        self.client.get("/api/fixCategories",
        name="get_categories")

    @tag('get_types', 'fix')
    @task(5)
    def get_types(self):
        self.client.get("/api/fixTypes",
        name="get_types")


    @tag('create_fix_template', 'fix')
    @task(1)
    def create_fix_template(self):
        self.client.post("/api/fixTemplates",
            json={
                "Status": "Public",
                "Name": "Template Name",
                "CategoryId": "2d6c6f6e-7732-44c5-901b-17a09a1b43f0",
                "TypeId": "bf83cad2-8108-4809-beae-3b1e9f63b681",
                "Description": "Please work",
                "SystemCostEstimate": 0.00,
                "CreatedByUserId": "0ba10288-fa9d-420c-bae3-b5326f773130",
                "UpdatedByUserId": "0ba10288-fa9d-420c-bae3-b5326f773130",
                "Tags": ["Tag1", "Tag2"],
                "Sections": [
                    {
                        "Name": "Section Name 1",
                        "Fields": [
                            {
                                "Name": "Field Name 1.1",
                                "Values": ["Value 1.1.1", "Value 1.1.2"]
                            },
                            {
                                "Name": "Field Name 1.2",
                                "Values": ["Value 1.2.1", "Value 1.2.2"]
                            }
                        ]
                    },
                    {
                        "Name": "Section Name 2",
                        "Fields": [
                            {
                                "Name": "Field Name 2.1",
                                "Values": ["Value 2.1.1", "Value 2.1.2"]
                            }
                        ]
                    }
                ]
            },
            name="create_fix_template")
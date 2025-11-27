import { employeeService } from "../api/services/recruitment/employeeService";
import { useEffect } from "react";

export const TestEmployeeApi = () => {
  useEffect(() => {
    const test = async () => {
      try {
        const result = await employeeService.getAllEmployees();
        console.log("Employees:", result);
      } catch (err) {
        console.error(err);
      }
    };

    test();
  }, []);

  return <div>Testing Employee API... Check console!</div>;
};

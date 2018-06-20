package hello;

import static org.hamcrest.Matchers.equalTo;
import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertNull;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;
import static org.springframework.test.web.servlet.setup.MockMvcBuilders.standaloneSetup;

import com.jayway.jsonpath.JsonPath;
import org.hamcrest.Matchers;
import org.junit.After;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.MediaType;
import org.springframework.mock.web.MockMultipartFile;
import org.springframework.test.context.junit4.SpringJUnit4ClassRunner;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.MvcResult;
import org.springframework.test.web.servlet.ResultActions;
import org.springframework.test.web.servlet.request.MockHttpServletRequestBuilder;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import java.io.FileInputStream;
import java.time.Clock;

@RunWith(SpringJUnit4ClassRunner.class)
@SpringBootTest
@AutoConfigureMockMvc
public class HelloControllerTest {

    private MockMvc mvc;


    @Mock
    public service ser;

    @InjectMocks
    public HelloController helloCont;

/*

    @Before
    public void setup() throws Exception{
        mvc = MockMvcBuilders.standaloneSetup(helloCont).build();
    }
    @Test
    public void getHello() throws Exception {
     mvc.perform(get("/create")).andExpect(status().isOk())
             .andExpect(content().string("Greetings from Spring Boot!"));

    }

    @Test
    public void TestBiologyClass() throws Exception {
        mvc.perform(get("/biology").accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.studentName", Matchers.is("Fucker")))
                .andExpect(jsonPath("$.studentAge",Matchers.is("25")));
    }

    @Test
    public void testPostBody() throws Exception {

        byte[] mockData = "test".getBytes();
        String originalFilename = "sample.out.zip";
        MockMultipartFile mockMultipartFile = new MockMultipartFile("file", originalFilename, "application/zip", mockData);

        MockHttpServletRequestBuilder builder =
                MockMvcRequestBuilders.multipart("/uploadFile")
                        .file(mockMultipartFile)
                .param("key","1537878e278378");


        MvcResult result = mvc.perform(builder)
                .andExpect(status().isOk())
                .andReturn();
        assertNotNull(result.getResponse().getContentAsString());
                //.andExpect(jsonPath("$.*", Matchers.hasSize(1)));
                //.andExpect(jsonPath("$.studentAge",Matchers.is("25")));
    }
*/

    @Autowired
    private UserMongoRepository userMongoRepository;

    @Before
    public void setUp() throws Exception {
         user user1= new user("Alice", 23);
        user user2= new user("Bob", 38);
        //save product, verify has ID value after save
        assertNull(user1.getId());
        assertNull(user2.getId());//null before save
        this.userMongoRepository.save(user1);
        this.userMongoRepository.save(user2);

        assertNotNull(user1.getId());
        assertNotNull(user2.getId());
    }


    @Test
    public void testFetchData(){
        /*Test data retrieval*/
        user userA = userMongoRepository.findByName("Bob");
        assertNotNull(userA);
        assertEquals(38, userA.getAge());
        /*Get all products, list should only have two*/
        Iterable<user> users = userMongoRepository.findAll();
        int count = 0;
        for(user p : users){
            count++;
        }
        assertEquals(count, 2);
    }



    @Test
    public void testDataUpdate(){
        /*Test update*/
        user userB = userMongoRepository.findByName("chan");
        userB.setAge(40);
        userMongoRepository.save(userB);
        user userC= userMongoRepository.findByName("chan");
        assertNotNull(userC);
        assertEquals(40, userC.getAge());
    }

    @After
    public void tearDown() throws Exception {
        //this.userMongoRepository.deleteAll();
    }
}
